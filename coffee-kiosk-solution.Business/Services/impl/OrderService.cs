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

        public OrderService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<IOrderService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OrderViewModel> Create(OrderCreateViewModel model)
        {
            var order = _mapper.Map<TblOrder>(model);

            try
            {
                await _unitOfWork.OrderRepository.InsertAsync(order);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.OrderRepository
                    .Get(p => p.Id.Equals(order.Id))
                    .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
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
            var discount = await _unitOfWork.OrderRepository
                .Get(p => p.Id.Equals(id))
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (discount == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            return discount;

        }
    }
}
