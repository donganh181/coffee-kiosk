using AutoMapper;
using AutoMapper.QueryableExtensions;
using coffee_kiosk_solution.Business.Utilities;
using coffee_kiosk_solution.Data.Constants;
using coffee_kiosk_solution.Data.Models;
using coffee_kiosk_solution.Data.Repositories;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services.impl
{
    public class OrderService : IOrderService
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IOrderService> _logger;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IDiscountService _discountService;

        public OrderService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork,
            ILogger<IOrderService> logger, IOrderDetailService orderDetailService,
            IDiscountService discountService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _orderDetailService = orderDetailService;
            _discountService = discountService;
        }

        public async Task<OrderViewModel> Create(OrderCreateViewModel model)
        {
            var order = _mapper.Map<TblOrder>(model);
            if (model.DiscountId != null) 
            {
                var discount = await _discountService.CheckDiscountByShopId(Guid.Parse(model.DiscountId + ""), model.ShopId);
                if(discount != null)
                {
                    if(model.TotalPriceBeforeDiscount < discount.RequiredValue)
                    {
                        _logger.LogError("Discount did not meet requirement.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Discount did not meet requirement.");
                    }
                    else
                    {
                        order.TotalPrice = model.TotalPriceBeforeDiscount - discount.DiscountValue;
                    }
                }
            }
            else
            {
                order.TotalPrice = model.TotalPriceBeforeDiscount;
            }

            order.Status = (int)OrderStatusConstants.Ordered;
            order.CreateDate = DateTime.Now;
            try
            {
                await _unitOfWork.OrderRepository.InsertAsync(order);
                await _unitOfWork.SaveAsync();
                foreach(var orderItem in model.ListOrder)
                {
                    var orderMapped = new OrderDetailCreateViewModel{
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                        Price = orderItem.Price,
                        OrderId = order.Id,
                        ShopId = order.ShopId
                    };
                    await _orderDetailService.Create(orderMapped);
                }
                var result = _unitOfWork.OrderRepository
                    .Get(p => p.Id.Equals(order.Id))
                    .Include(a => a.TblOrderDetails.Where(x => x.OrderId.Equals(order.Id)))
                    .ToList()
                    .AsQueryable()
                    .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                _unitOfWork.OrderRepository.Delete(order);
                await _unitOfWork.SaveAsync();
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }

        public async Task<DynamicModelResponse<OrderSearchViewModel>> GetAllWithPaging(OrderSearchViewModel model, int size, int pageNum)
        {
            var listOrder = _unitOfWork.OrderRepository
                .Get()
                .ProjectTo<OrderSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable();

            var listPaging = listOrder
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<OrderSearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<OrderViewModel> GetById(Guid id)
        {
            var result = await _unitOfWork.OrderRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(a => a.TblOrderDetails.Where(x => x.OrderId.Equals(id)))
                    .ToList()
                    .AsQueryable()
                    .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

            if (result == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            return result;

        }
    }
}
