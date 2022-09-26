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
    public class CampaignService : ICampaignService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ICampaignService> _logger;

        public CampaignService(IMapper mapper, IConfiguration configuration,
            IUnitOfWork unitOfWork, ILogger<ICampaignService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CampaignViewModel> ChangeStatus(Guid id)
        {
            var campaign = await _unitOfWork.CampaignRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (campaign == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign is deleted.");
            }
            else if (campaign.Status == (int)StatusConstants.Activate)
            {
                campaign.Status = (int)StatusConstants.Deactivate;
            }
            else if(campaign.Status == (int)StatusConstants.Deactivate && 
                DateTime.Compare(campaign.ExpiredDate, DateTime.Now) < 0)
            {
                _logger.LogError("This campaign cannot change status because it is expired.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign cannot change status because it is expired.");
            }
            else
            {
                campaign.Status = (int)StatusConstants.Activate;
            }
            try
            {
                _unitOfWork.CampaignRepository.Update(campaign);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.CampaignRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(a => a.Area)
                    .ProjectTo<CampaignViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;

            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }

        public async Task<CampaignViewModel> Create(CampaignCreateViewModle model)
        {
            var campaign = _mapper.Map<TblCampaign>(model);

            if (campaign.ExpiredDate <= DateTime.Now)
            {
                _logger.LogError("Invalid Expired Date");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Expired Date");
            }
            if (campaign.ExpiredDate <= campaign.StartingDate)
            {
                _logger.LogError("Invalid Expired Date and Starting Date");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Expired Date and Starting Date");
            }

            campaign.Status = (int)StatusConstants.Activate;

            try
            {
                await _unitOfWork.CampaignRepository.InsertAsync(campaign);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.CampaignRepository
                    .Get(p => p.Id.Equals(campaign.Id))
                    .Include(a => a.Area)
                    .ProjectTo<CampaignViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {

                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");

            }
        }

        public async Task<CampaignViewModel> Delete(Guid id)
        {
            var campaign = await _unitOfWork.CampaignRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (campaign == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign is deleted.");
            }
            campaign.Status = (int)StatusConstants.Deleted;
            campaign.Name = campaign.Name + $"-{DateTime.Now}-Deleted";
            try
            {
                _unitOfWork.CampaignRepository.Update(campaign);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.CampaignRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(a => a.Area)
                    .ProjectTo<CampaignViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }

        }

        public async Task<DynamicModelResponse<CampaignSearchViewModel>> GetAllWithPaging(CampaignSearchViewModel model, int size, int pageNum)
        {
            if (model.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("Cannot search campaign which is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot search campaign which is deleted.");
            }

            var listCampaign = _unitOfWork.CampaignRepository
                .Get()
                .Include(a => a.Area)
                .ProjectTo<CampaignSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable()
                .OrderByDescending(l => l.Name);

            var listPaging = listCampaign
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }

            var result = new DynamicModelResponse<CampaignSearchViewModel>
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

        public async Task<CampaignViewModel> GetById(Guid id)
        {
            var campaign = await _unitOfWork.CampaignRepository
                .Get(p => p.Id.Equals(id))
                .Include(a => a.Area)
                .ProjectTo<CampaignViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (campaign == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign is deleted.");
            }
            return campaign;
        }

        public async Task<List<CampaignViewModel>> GetListCampaignInTheSameTime(Guid areaId, DateTime startingDate, DateTime expiredDate)
        {
            var listCampaign = await _unitOfWork.CampaignRepository
                .Get(c => c.AreaId.Equals(areaId) 
                        && c.Status == (int)StatusConstants.Activate 
                        && ((startingDate >= c.StartingDate && expiredDate <= c.ExpiredDate)
                        || (c.StartingDate >= startingDate && c.ExpiredDate <= expiredDate)))
                .ProjectTo<CampaignViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return listCampaign;
        }

        public async Task<CampaignViewModel> Update(CampaignUpdateViewModel model)
        {
            var campaign = await _unitOfWork.CampaignRepository
                .Get(p => p.Id.Equals(model.Id))
                .Include(a => a.Area)
                .FirstOrDefaultAsync();
            if (campaign == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign is deleted.");
            }

            if (campaign.ExpiredDate <= DateTime.Now)
            {
                _logger.LogError("Invalid Expired Date");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Expired Date");
            }
            if (campaign.ExpiredDate <= campaign.StartingDate)
            {
                _logger.LogError("Invalid Expired Date and Starting Date");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Expired Date and Starting Date");
            }

            campaign.Name = model.Name;
            campaign.Description = model.Description;
            campaign.StartingDate = model.StartingDate;
            campaign.ExpiredDate = model.ExpiredDate;
            campaign.AreaId = model.AreaId;

            try
            {
                _unitOfWork.CampaignRepository.Update(campaign);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.CampaignRepository
                    .Get(p => p.Id.Equals(model.Id))
                    .Include(a => a.Area)
                    .ProjectTo<CampaignViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    _logger.LogError("Name is duplicated.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Name is duplicated.");
                }
                else
                {
                    _logger.LogError("Invalid data.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
                }
            }
        }

        public async Task<bool> ValidateStatusOfCampaignByDay()
        {
            var now = DateTime.Now;
            var listCampaign = await _unitOfWork.CampaignRepository
                .Get(c => c.Status != (int)StatusConstants.Deactivate 
                        && c.Status != (int)StatusConstants.Deleted
                        && DateTime.Compare((DateTime)c.ExpiredDate, now) < 0)
                .ToListAsync();
            try
            {
                foreach(var campaignUpdate in listCampaign)
                {
                    campaignUpdate.Status = (int)StatusConstants.Deactivate;

                    _unitOfWork.CampaignRepository.Update(campaignUpdate);
                    await _unitOfWork.SaveAsync();
                    _logger.LogInformation($"Campaign {campaignUpdate.Name} Status has been changed to Deactivate");
                }
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}
