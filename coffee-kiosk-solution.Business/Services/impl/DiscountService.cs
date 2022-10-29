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
    public class DiscountService : IDiscountService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IDiscountService> _logger;
        private readonly ICampaignService _campaignService;
        private readonly IProductService _productService;

        public DiscountService(IMapper mapper, IConfiguration configuration,
            IUnitOfWork unitOfWork, ILogger<IDiscountService> logger,
            ICampaignService campaignService, IProductService productService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _campaignService = campaignService;
            _productService = productService;
        }

        public async Task<DiscountViewModel> ChangeStatus(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (discount == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            if (discount.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This discount is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This discount is deleted.");
            }
            else if (discount.Status == (int)StatusConstants.Activate)
            {
                discount.Status = (int)StatusConstants.Deactivate;
            }
            else
            {
                discount.Status = (int)StatusConstants.Activate;
            }

            try
            {
                _unitOfWork.DiscountRepository.Update(discount);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.DiscountRepository
                    .Get(p => p.Id.Equals(id))
                    .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DiscountViewModel> Create(DiscountCreateViewModel model)
        {
            var discount = _mapper.Map<TblDiscount>(model);
            var campaign = await _campaignService.GetById(model.CampaignId);
            var product = await _productService.GetById(model.ProductId);

            var listCheck = await _campaignService.GetListCampaignInTheSameTime(campaign.AreaId, campaign.StartingDate, campaign.ExpiredDate);
            if (listCheck.Count > 0)
            {
                foreach (var check in listCheck)
                {
                    var listDiscount = await GetListDiscountByCampaign(check.Id);
                    if (listDiscount.Count > 0)
                    {
                        foreach (var item in listDiscount)
                        {
                            if (item.ProductId.Equals(model.ProductId))
                            {
                                _logger.LogError("This product has been set in another campaign in the same time.");
                                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product has been set in another campaign in the same time.");
                            }
                        }
                    }
                }
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("The Campaign selected has been deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "The Campaign selected has been deleted.");
            }

            if (product.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("The Product selected has been deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "The Product selected has been deleted.");
            }

            discount.Status = (int)StatusConstants.Activate;

            try
            {
                await _unitOfWork.DiscountRepository.InsertAsync(discount);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.DiscountRepository
                    .Get(p => p.Id.Equals(discount.Id))
                    .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }

        public async Task<DiscountViewModel> Delete(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (discount == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (discount.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This discount is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This discount is deleted.");
            }
            discount.Status = (int)StatusConstants.Deleted;

            try
            {
                _unitOfWork.DiscountRepository.Update(discount);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.DiscountRepository
                    .Get(p => p.Id.Equals(id))
                    .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }

        public async Task<DynamicModelResponse<DiscountSearchViewModel>> GetAllWithPaging(DiscountSearchViewModel model, int size, int pageNum)
        {
            if (model.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("Cannot search discount which is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot find discount which is deleted.");
            }

            var listDiscount = _unitOfWork.DiscountRepository
                .Get()
                .Include(a => a.Product)
                .Include(b => b.Campaign)
                .ProjectTo<DiscountSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable();

            var listPaging = listDiscount
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<DiscountSearchViewModel>
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

        public async Task<DiscountViewModel> GetById(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository
                .Get(p => p.Id.Equals(id))
                .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (discount == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (discount.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This discount is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This discount is deleted.");
            }
            return discount;
        }

        public async Task<List<DiscountViewModel>> GetListDiscountByCampaign(Guid campaignId)
        {
            var listDiscount = await _unitOfWork.DiscountRepository
                .Get(d => d.CampaignId.Equals(campaignId))
                .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return listDiscount;
        }

        public async Task<DiscountViewModel> Update(DiscountUpdateViewModel model)
        {
            var discount = await _unitOfWork.DiscountRepository
                .Get(p => p.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (discount == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (discount.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This discount is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This discount is deleted.");
            }

            var campaign = await _campaignService.GetById(model.CampaignId);
            var product = await _productService.GetById(model.ProductId);

            var listCheck = await _campaignService.GetListCampaignInTheSameTime(campaign.AreaId, campaign.StartingDate, campaign.ExpiredDate);
            if (listCheck.Count > 0)
            {
                foreach (var check in listCheck)
                {
                    var listDiscount = await GetListDiscountByCampaign(check.Id);
                    if (listDiscount.Count > 0)
                    {
                        foreach (var item in listDiscount)
                        {
                            if (item.ProductId.Equals(model.ProductId))
                            {
                                _logger.LogError("This product has been set in another campaign in the same time.");
                                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product has been set in another campaign in the same time.");
                            }
                        }
                    }
                }
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("The Campaign selected has been deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "The Campaign selected has been deleted.");
            }

            if (product.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("The Product selected has been deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "The Product selected has been deleted.");
            }

            discount.DiscountPercentage = model.DiscountPercentage;
            discount.ProductId = model.ProductId;
            discount.CampaignId = model.CampaignId;
            try
            {
                _unitOfWork.DiscountRepository.Update(discount);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.DiscountRepository
                    .Get(p => p.Id.Equals(model.Id))
                    .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }
    }
}
