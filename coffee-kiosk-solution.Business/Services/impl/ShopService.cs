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
    public class ShopService : IShopService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IShopService> _logger;

        public ShopService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IShopService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ShopViewModel> ChangeShopManager(Guid shopId, Guid? managerId)
        {
            
            var shop = await _unitOfWork.ShopRepository
                .Get(s => s.Id.Equals(shopId))
                .FirstOrDefaultAsync();

            if(shop == null)
            {
                _logger.LogError("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }

            if(shop.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This shop is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This shop is deleted.");
            }
            if (!managerId.Equals(Guid.Empty))
            {
                shop.AccountId = managerId;
            }
            else
            {
                shop.AccountId = Guid.Empty;
            }

            var check = await _unitOfWork.ShopRepository
                .Get(c => c.AccountId.Equals(Guid.Parse(managerId+"")) && c.Status != (int)StatusConstants.Deleted)
                .FirstOrDefaultAsync();
            if (check != null)
            {
                _logger.LogError($"This staff managed shop with id: {check.Id}.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"This staff managed shop with id: {check.Id}.");
            }

            try
            {
                _unitOfWork.ShopRepository.Update(shop);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ShopRepository
                    .Get(s => s.Id.Equals(shopId))
                    .Include(b => b.Area)
                    .ProjectTo<ShopViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<ShopViewModel> ChangeStatus(Guid id)
        {
            var shop = await _unitOfWork.ShopRepository
                .Get(s => s.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (shop == null)
            {
                _logger.LogError("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }

            if (shop.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This shop is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This shop is deleted.");
            }

            else if(shop.Status == (int)StatusConstants.Activate)
            {
                shop.Status = (int)StatusConstants.Deactivate;
            }
            else
            {
                shop.Status = (int)StatusConstants.Activate;
            }

            try
            {
                _unitOfWork.ShopRepository.Update(shop);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ShopRepository
                    .Get(s => s.Id.Equals(id))
                    .Include(b => b.Area)
                    .ProjectTo<ShopViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<ShopViewModel> Create(ShopCreateViewModel model)
        {
            var shop = _mapper.Map<TblShop>(model);

            var check = await _unitOfWork.ShopRepository
                .Get(c => c.AccountId.Equals(model.AccountId) && c.Status != (int)StatusConstants.Deleted)
                .FirstOrDefaultAsync();
            if(check != null)
            {
                _logger.LogError($"This staff managed shop with id: {check.Id}.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"This staff managed shop with id: {check.Id}.");
            }

            shop.Status = (int)StatusConstants.Activate;

            try
            {
                await _unitOfWork.ShopRepository.InsertAsync(shop);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ShopRepository
                    .Get(s => s.Id.Equals(shop.Id))
                    .Include(b => b.Area)
                    .ProjectTo<ShopViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;

            }
            catch(Exception e)
            {
                if (e.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    _logger.LogError("Name or address is duplicated.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Name or address is duplicated.");
                }
                else
                {
                    _logger.LogError("Invalid data.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
                }
            }
        }

        public async Task<ShopViewModel> Delete(Guid id)
        {
            var shop = await _unitOfWork.ShopRepository
                .Get(s => s.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (shop == null)
            {
                _logger.LogError("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }

            if (shop.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This shop is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This shop is deleted.");
            }

            shop.Status = (int)StatusConstants.Deleted;
            shop.Name = shop.Name + $" - {DateTime.Now} - Deleted";
            shop.Address = shop.Address + $" - {DateTime.Now} - Deleted";
            try
            {
                _unitOfWork.ShopRepository.Update(shop);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ShopRepository
                    .Get(s => s.Id.Equals(id))
                    .Include(b => b.Area)
                    .ProjectTo<ShopViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<ShopSearchViewModel>> GetAllWithPaging(ShopSearchViewModel model, int size, int pageNum, string role, Guid managerId)
        {
            if (model.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("Cannot search product which is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot search product which is deleted.");
            }
            IQueryable<ShopSearchViewModel> listShop = null;
            if (role.Equals(RoleConstants.ADMIN))
            {
                listShop = _unitOfWork.ShopRepository
                .Get(s => s.Status != (int)StatusConstants.Deleted)
                .Include(a => a.Account)
                .Include(b => b.Area)
                .ProjectTo<ShopSearchViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter(model)
                .AsQueryable()
                .OrderByDescending(p => p.Name);
            }
            else
            {
                listShop = _unitOfWork.ShopRepository
                .Get(s => s.Status != (int)StatusConstants.Deleted
                    && s.AccountId.Equals(managerId))
                .Include(a => a.Account)
                .Include(b => b.Area)
                .ProjectTo<ShopSearchViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter(model)
                .AsQueryable()
                .OrderByDescending(p => p.Name);
            }

            if(listShop.ToList().Count < 1)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }

            var listPaging = listShop
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            var result = new DynamicModelResponse<ShopSearchViewModel>
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

        public async Task<Guid> GetAreaIdByShopId(Guid shopId)
        {
            var shop = await _unitOfWork.ShopRepository
                .Get(s => s.Id.Equals(shopId))
                .FirstOrDefaultAsync();

            return shop.AreaId;
        }

        public async Task<ShopViewModel> GetById(Guid id, string role, Guid managerId)
        {
            var shop = await _unitOfWork.ShopRepository
                    .Get(s => s.Id.Equals(id))
                    .Include(b => b.Area)
                    .ProjectTo<ShopViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

            if (shop == null)
            {
                _logger.LogError("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }
            if (!role.Equals(RoleConstants.ADMIN) && !shop.AccountId.Equals(managerId))
            {
                _logger.LogError("This shop is not belong to this manager.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This shop is not belong to this manager.");
            }
            if (shop.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This shop is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This shop is deleted.");
            }
            return shop;
        }

        public async Task<ShopViewModel> Update(ShopUpdateViewModel model)
        {
            var shop = await _unitOfWork.ShopRepository
                .Get(s => s.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (shop == null)
            {
                _logger.LogError("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }

            if (shop.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This shop is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This shop is deleted.");
            }

            shop.Name = model.Name;
            shop.Description = model.Description;
            shop.Address = model.Address;
            shop.AreaId = model.AreaId;
            try
            {
                _unitOfWork.ShopRepository.Update(shop);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ShopRepository
                    .Get(s => s.Id.Equals(shop.Id))
                    .Include(b => b.Area)
                    .ProjectTo<ShopViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;

            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    _logger.LogError("Name or address is duplicated.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Name or address is duplicated.");
                }
                else
                {
                    _logger.LogError("Invalid data.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
                }
            }
        }
    }
}
