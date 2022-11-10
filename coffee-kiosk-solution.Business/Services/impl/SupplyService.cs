using AutoMapper;
using AutoMapper.QueryableExtensions;
using coffee_kiosk_solution.Business.Utilities;
using coffee_kiosk_solution.Data.Constants;
using coffee_kiosk_solution.Data.Models;
using coffee_kiosk_solution.Data.Repositories;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services.impl
{
    public class SupplyService : ISupplyService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ISupplyService> _logger;

        public SupplyService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<ISupplyService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SupplyViewModel> ChangeStatus(Guid managerId, Guid supplyId)
        {
            var supply = await _unitOfWork.SupplyRepository
                .Get(s => s.Id.Equals(supplyId))
                .Include(a => a.Shop)
                .ThenInclude(b => b.Account)
                .FirstOrDefaultAsync();
            if(supply == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!supply.Shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This supply is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is not belong to this manager.");
            }
            if (supply.Status == (int)SupplyStatusConstants.Stopped)
            {
                _logger.LogError("This supply is stopped.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is stopped.");
            }
            else if (supply.Status == (int)SupplyStatusConstants.Available)
            {
                supply.Status = (int)SupplyStatusConstants.Unavailable;
            }
            else
            {
                supply.Status = (int)SupplyStatusConstants.Available;
            }
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supplyId))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }

        public async Task<bool> CheckSupplyByWeek()
        {
            var listSupply = await _unitOfWork.SupplyRepository
                .Get(s => s.Status != (int)SupplyStatusConstants.Stopped)
                .ToListAsync();
            try
            {
                foreach (var supply in listSupply)
                {
                    supply.SupplyQuantity = supply.Quantity;
                    supply.Status = (int)SupplyStatusConstants.Available;
                    _unitOfWork.SupplyRepository.Update(supply);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
            
        }

        public async Task<SupplyViewModel> CreateNew(Guid managerId, SupplyCreateViewModel model)
        {
            var supply = _mapper.Map<TblSupply>(model);

            supply.SupplyQuantity = model.Quantity;
            supply.Status = (int)SupplyStatusConstants.Available;
            try
            {
                await _unitOfWork.SupplyRepository.InsertAsync(supply);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supply.Id))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<SupplySearchViewModel>> GetAllWithPaging(Guid managerId, SupplySearchViewModel model, int size, int pageNum)
        {
            var listProduct = _unitOfWork.SupplyRepository
                .Get(p => p.Shop.AccountId.Equals(managerId))
                .Include(a => a.Shop)
                .Include(b => b.Product)
                .ThenInclude(c => c.TblProductImages)
                .ProjectTo<SupplySearchViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            var listPaging = listProduct
                .AsQueryable()
                .OrderByDescending(p => p.ProductName)
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }
            var result = new DynamicModelResponse<SupplySearchViewModel>
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

        public async Task<DynamicModelResponse<SupplyCustomerSearchViewModel>> GetAllWithPagingByShopId(Guid shopId, SupplyCustomerSearchViewModel model, int size, int pageNum)
        {
            var listProduct = _unitOfWork.SupplyRepository
                .Get(p => p.ShopId.Equals(shopId))
                .Include(a => a.Shop)
                .Include(b => b.Product)
                .ThenInclude(c => c.TblProductImages)
                .Include(b => b.Product)
                .ThenInclude(x => x.Category)
                .ProjectTo<SupplyCustomerSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            var listPaging = listProduct
                .AsQueryable()
                .OrderByDescending(p => p.ProductName)
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }
            var result = new DynamicModelResponse<SupplyCustomerSearchViewModel>
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

        public async Task<SupplyViewModel> GetById(Guid managerId, Guid supplyId)
        {
            var check = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supplyId))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .FirstOrDefaultAsync();
            if (check == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!check.Shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This supply is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This supply is not belong to this manager.");
            }
            var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supplyId))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> GetQuantityByShopIdAndProductId(Guid shopId, Guid productId)
        {
            var supply = await _unitOfWork.SupplyRepository
                .Get(s => s.ShopId.Equals(shopId) && s.ProductId.Equals(productId))
                .FirstOrDefaultAsync();
            if(supply.Status != (int)SupplyStatusConstants.Available)
            {
                _logger.LogError("Product is not available.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Product is not available.");
            }
            if(supply == null)
            {
                return 0;
            }
            return supply.SupplyQuantity;
        }

        public async Task<SupplyViewModel> ReImport(Guid managerId, Guid supplyId)
        {
            var supply = await _unitOfWork.SupplyRepository
               .Get(s => s.Id.Equals(supplyId))
               .Include(a => a.Shop)
               .ThenInclude(b => b.Account)
               .FirstOrDefaultAsync();
            if (supply == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!supply.Shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This supply is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is not belong to this manager.");
            }
            if (supply.Status != (int)SupplyStatusConstants.Stopped)
            {
                _logger.LogError("This supply is not stopped.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is not stopped.");
            }

            supply.Status = (int)SupplyStatusConstants.Unavailable;
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supplyId))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<SupplyViewModel> StopSupply(Guid managerId, Guid supplyId)
        {
            var supply = await _unitOfWork.SupplyRepository
               .Get(s => s.Id.Equals(supplyId))
               .Include(a => a.Shop)
               .ThenInclude(b => b.Account)
               .FirstOrDefaultAsync();
            if (supply == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!supply.Shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This supply is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is not belong to this manager.");
            }
            if (supply.Status == (int)SupplyStatusConstants.Stopped)
            {
                _logger.LogError("This supply is stopped.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is stopped.");
            }
            supply.Status = (int)SupplyStatusConstants.Stopped;
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supplyId))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<SupplyViewModel> UpdateQuantity(Guid managerId, SupplyUpdateQuantityViewModel model)
        {
            var supply = await _unitOfWork.SupplyRepository
               .Get(s => s.Id.Equals(model.Id))
               .Include(a => a.Shop)
               .ThenInclude(b => b.Account)
               .FirstOrDefaultAsync();
            if (supply == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!supply.Shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This supply is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is not belong to this manager.");
            }

            supply.Quantity = model.Quantity;
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supply.Id))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<bool> UpdateSupplyAfterCancelOrder(Guid shopId, Guid productId, int quantity)
        {
            var supply = await _unitOfWork.SupplyRepository
                .Get(s => s.ShopId.Equals(shopId) && s.ProductId.Equals(productId))
                .FirstOrDefaultAsync();

            supply.SupplyQuantity = supply.SupplyQuantity + quantity;
            if (supply.SupplyQuantity > 0)
            {
                supply.Status = (int)SupplyStatusConstants.Available;
            }
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<bool> UpdateSupplyAfterOrder(Guid shopId, Guid productId, int quantity)
        {
            var supply = await _unitOfWork.SupplyRepository
                .Get(s => s.ShopId.Equals(shopId) && s.ProductId.Equals(productId))
                .FirstOrDefaultAsync();

            supply.SupplyQuantity = supply.SupplyQuantity - quantity;
            if(supply.SupplyQuantity == 0)
            {
                supply.Status = (int)SupplyStatusConstants.Unavailable;
            }
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<SupplyViewModel> UpdateSupplyQuantity(Guid managerId, SupplyUpdateSupQuantityViewModel model)
        {
            var supply = await _unitOfWork.SupplyRepository
              .Get(s => s.Id.Equals(model.Id))
              .Include(a => a.Shop)
              .ThenInclude(b => b.Account)
              .FirstOrDefaultAsync();
            if (supply == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!supply.Shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This supply is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This supply is not belong to this manager.");
            }

            supply.SupplyQuantity = model.SupQuantity;
            try
            {
                _unitOfWork.SupplyRepository.Update(supply);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.SupplyRepository
                    .Get(s => s.Id.Equals(supply.Id))
                    .Include(a => a.Shop)
                    .Include(b => b.Product)
                    .ThenInclude(c => c.TblProductImages)
                    .ProjectTo<SupplyViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}
