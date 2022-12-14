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
    public class AreaService : IAreaService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAreaService> _logger;

        public AreaService(IMapper mapper, IUnitOfWork unitOfWork,
            ILogger<IAreaService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AreaViewModel> Create(AreaCreateViewModel model)
        {
            var area = _mapper.Map<TblArea>(model);
            try
            {
                await _unitOfWork.AreaRepository.InsertAsync(area);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.AreaRepository
                    .Get(p => p.Id.Equals(area.Id))
                    .ProjectTo<AreaViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public Task<AreaViewModel> Delete(Guid id)
        {
            throw new NotImplementedException();
        }



        public async Task<DynamicModelResponse<AreaSearchViewModel>> GetAllWithPaging(AreaSearchViewModel model, int size, int pageNum)
        {

            var listArea = _unitOfWork.AreaRepository
                .Get()
                .ProjectTo<AreaSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable()
                .OrderByDescending(l => l.AreaName);

            var listPaging = listArea
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }

            var result = new DynamicModelResponse<AreaSearchViewModel>
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

        public async Task<AreaViewModel> GetById(Guid id)
        {
            var area = await _unitOfWork.AreaRepository
                .Get(p => p.Id.Equals(id))
                .ProjectTo<AreaViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (area == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            return area;
        }

        public async Task<AreaViewModel> Update(AreaUpdateViewModel model)
        {
            var area = await _unitOfWork.AreaRepository
                .Get(p => p.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (area == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            area.AreaName = model.AreaName;

            try
            {
                _unitOfWork.AreaRepository.Update(area);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.AreaRepository
                    .Get(p => p.Id.Equals(model.Id))
                    .ProjectTo<AreaViewModel>(_mapper.ConfigurationProvider)
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
