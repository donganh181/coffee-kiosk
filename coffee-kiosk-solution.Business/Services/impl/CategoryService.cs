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
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ICategoryService> _logger;

        public CategoryService(IMapper mapper, IConfiguration configuration,
            IUnitOfWork unitOfWork, ILogger<ICategoryService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CategoryViewModel> Create(CategoryCreateViewModel model)
        {
            var cate = _mapper.Map<TblCategory>(model);
            cate.Status = (int)StatusConstants.Activate;

            try
            {
                await _unitOfWork.CategoryRepository.InsertAsync(cate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<CategoryViewModel>(cate);
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

        public async Task<CategoryViewModel> ChangeStatus(Guid id)
        {
            var cate = await _unitOfWork.CategoryRepository
                .Get(c => c.Id.Equals(id))
                .FirstOrDefaultAsync();

            if(cate.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This category is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This category is deleted.");
            }
            else if (cate.Status == (int)StatusConstants.Activate)
            {
                cate.Status = (int)StatusConstants.Deactivate;
            }
            else
            {
                cate.Status = (int)StatusConstants.Activate;
            }

            try
            {
                _unitOfWork.CategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<CategoryViewModel>(cate);
                return result;
            }
            catch(Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<CategoryViewModel> Delete(Guid id)
        {
            var cate = await _unitOfWork.CategoryRepository
                .Get(c => c.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (cate.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This category is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This category is deleted.");
            }
            else
            {
                cate.Status = (int)StatusConstants.Deleted;
            }

            cate.Name = cate.Name + $"-{DateTime.Now}-Deleted";
            try
            {
                _unitOfWork.CategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<CategoryViewModel>(cate);
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<CategoryViewModel> Update(CategoryUpdateViewModel model)
        {
            var cate = await _unitOfWork.CategoryRepository
                .Get(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (cate.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This category is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This category is deleted.");
            }

            cate.Name = model.Name;
            try
            {
                _unitOfWork.CategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<CategoryViewModel>(cate);
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

        public async Task<CategoryViewModel> GetById(Guid id)
        {
            var cate = await _unitOfWork.CategoryRepository
                .Get(c => c.Id.Equals(id))
                .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (cate.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This category is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This category is deleted.");
            }

            return cate;
        }

        public async Task<DynamicModelResponse<CategorySearchViewModel>> GetAllWithPaging(CategorySearchViewModel model, int size, int pageNum)
        {
            if(model.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("Cannot search category which is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot search category which is deleted.");
            }
            var listCate = _unitOfWork.CategoryRepository
                .Get()
                .ProjectTo<CategorySearchViewModel>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable()
                .OrderByDescending(l => l.Name);

            var listPaging = listCate
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<CategorySearchViewModel>
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
    }
}
