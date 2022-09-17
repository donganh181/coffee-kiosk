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
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product is deleted.");
            }
            else if (campaign.Status == (int)StatusConstants.Activate)
            {
                campaign.Status = (int)StatusConstants.Deactivate;
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

            if (campaign.StartingDate > DateTime.Now)
            {
                _logger.LogError("Invalid Starting Date");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Start Date");
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

        public async Task<CampaignViewModel> Delete(Guid id)
        {
            var campaign = await _unitOfWork.CampaignRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (campaign == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign is deleted.");
            }
            campaign.Status = (int)StatusConstants.Deleted;

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

        public async Task<DynamicModelResponse<CampaignSearchViewModule>> GetAllWithPaging(CampaignSearchViewModule model, int size, int pageNum)
        {
            if (model.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("Cannot search product which is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot search product which is deleted.");
            }

            var listCampaign = _unitOfWork.CampaignRepository
                .Get()
                .Include(a => a.Area)
                .ProjectTo<CampaignSearchViewModule>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable()
                .OrderByDescending(l => l.Name);

            var listPaging = listCampaign
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<CampaignSearchViewModule>
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
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This campaign is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This campaign is deleted.");
            }
            return campaign;
        }

        public async Task<CampaignViewModel> Update(CampaignUpdateViewModel model)
        {
            var campaign = await _unitOfWork.CampaignRepository
                .Get(p => p.Id.Equals(model.Id))
                .Include(a => a.Area)
                .FirstOrDefaultAsync();
            if (campaign == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (campaign.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This product is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product is deleted.");
            }

            if (campaign.StartingDate > DateTime.Now)
            {
                _logger.LogError("Invalid Starting Date");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Start Date");
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
            //campaign.Status = model.Status;
            campaign.StartingDate = model.StartingDate;
            campaign.ExpiredDate = model.ExpiredDate;
            //campaign.AreaId = model.AreaId;

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
    }
}
