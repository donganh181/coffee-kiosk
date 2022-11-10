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
    public class OrderDetailService : IOrderDetailService
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IOrderDetailService> _logger;
        private readonly ISupplyService _supplyService;

        public OrderDetailService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork,
            ILogger<IOrderDetailService> logger, ISupplyService supplyService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _supplyService = supplyService;
        }

        public async Task<OrderDetailViewModel> Create(OrderDetailCreateViewModel model)
        {
            var orderDetail = _mapper.Map<TblOrderDetail>(model);
            var supply = await _supplyService.GetQuantityByShopIdAndProductId(model.ShopId, model.ProductId);
            
            if(model.Quantity > supply)
            {
                _logger.LogError("Cannot provide enough product.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot provide enough product.");
            }
            try
            {
                await _unitOfWork.OrderDetailRepository.InsertAsync(orderDetail);
                await _unitOfWork.SaveAsync();

                var supplyAfterOrder = await _supplyService.UpdateSupplyAfterOrder(model.ShopId, model.ProductId, model.Quantity);
                if (!supplyAfterOrder)
                {
                    _logger.LogError("Server error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server error.");
                }

                var result = await _unitOfWork.OrderDetailRepository
                    .Get(p => p.Id.Equals(orderDetail.Id))
                    .Include(a => a.Order)
                    .ProjectTo<OrderDetailViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<bool> Delete(Guid orderId, Guid shopId)
        {
            var listOrderDetail = await _unitOfWork.OrderDetailRepository
                .Get(o => o.OrderId.Equals(orderId))
                .ToListAsync();
            if(listOrderDetail.Count == 0)
            {
                return false;
            }
            try
            {
                foreach(var orderDetail in listOrderDetail)
                {
                    _unitOfWork.OrderDetailRepository.Delete(orderDetail);
                    await _unitOfWork.SaveAsync();
                    await _supplyService.UpdateSupplyAfterCancelOrder(shopId, orderDetail.ProductId, orderDetail.Quantity);
                    _logger.LogInformation($"Remove order detail id {orderDetail.Id} from order id {orderDetail.OrderId}");
                }
                _logger.LogInformation($"Remove all order from order ID {orderId}");
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<OrderDetailSearchViewModel>> GetAllWithPaging(OrderDetailSearchViewModel model, int size, int pageNum)
        {
            var listOrderDetail = _unitOfWork.OrderDetailRepository
                .Get()
                .Include(a => a.Order)
                .ProjectTo<OrderDetailSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            var listPaging = listOrderDetail
                .AsQueryable()
                .OrderByDescending(p => p.OrderId)
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }

            var result = new DynamicModelResponse<OrderDetailSearchViewModel>
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

        public async Task<OrderDetailViewModel> GetById(Guid id)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository
                .Get(p => p.Id.Equals(id))
                .Include(a => a.Order)
                .ProjectTo<OrderDetailViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (orderDetail == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            return orderDetail;

        }

        public async Task<List<OrderDetailViewModel>> GetListOrderDetailByOrderId(Guid orderId)
        {
            var listOrder = await _unitOfWork.OrderDetailRepository
                .Get(o => o.OrderId.Equals(orderId))
                .Include(a => a.Product)
                .ProjectTo<OrderDetailViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (listOrder.Count < 1)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return listOrder;
        }
    }
}
