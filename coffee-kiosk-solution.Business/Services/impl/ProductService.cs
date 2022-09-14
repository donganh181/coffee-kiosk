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
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IProductService> _logger;

        public ProductService(IMapper mapper, IConfiguration configuration,
            IUnitOfWork unitOfWork, ILogger<IProductService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProductViewModel> ChangeStatus(Guid id)
        {
            var product = await _unitOfWork.ProductRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (product == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if(product.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This product is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product is deleted.");
            }
            else if (product.Status == (int)StatusConstants.Activate)
            {
                product.Status = (int)StatusConstants.Deactivate;
            }
            else
            {
                product.Status = (int)StatusConstants.Activate;
            }
            try
            {
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ProductRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(a => a.Category)
                    .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<ProductViewModel> Create(ProductCreateViewModel model)
        {
            var product = _mapper.Map<TblProduct>(model);
            product.Status = (int)StatusConstants.Activate;
            try
            {
                await _unitOfWork.ProductRepository.InsertAsync(product);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ProductRepository
                    .Get(p => p.Id.Equals(product.Id))
                    .Include(a => a.Category)
                    .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch(Exception e)
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

        public async Task<ProductViewModel> Delete(Guid id)
        {
            var product = await _unitOfWork.ProductRepository
                .Get(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (product == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (product.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This product is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product is deleted.");
            }
            product.Status = (int)StatusConstants.Deleted;
            try
            {
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ProductRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(a => a.Category)
                    .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<ProductSearchViewModel>> GetAllWithPaging(ProductSearchViewModel model, int size, int pageNum)
        {
            if (model.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("Cannot search product which is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot search product which is deleted.");
            }
            var listProduct = _unitOfWork.ProductRepository
                .Get()
                .Include(a => a.Category)
                .ProjectTo<ProductSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList()
                .AsQueryable()
                .OrderByDescending(l => l.Name);

            var listPaging = listProduct
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<ProductSearchViewModel>
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

        public async Task<ProductViewModel> GetById(Guid id)
        {
            var product = await _unitOfWork.ProductRepository
                .Get(p => p.Id.Equals(id))
                .Include(a => a.Category)
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (product.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This product is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product is deleted.");
            }
            return product;
        }

        public async Task<ProductViewModel> Update(ProductUpdateViewModel model)
        {
            var product = await _unitOfWork.ProductRepository
                .Get(p => p.Id.Equals(model.Id))
                .Include(a => a.Category)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                _logger.LogError("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (product.Status == (int)StatusConstants.Deleted)
            {
                _logger.LogError("This product is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This product is deleted.");
            }
            product.Name = model.Name;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Price = model.Price;
            try
            {
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ProductRepository
                    .Get(p => p.Id.Equals(model.Id))
                    .Include(a => a.Category)
                    .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch(Exception e)
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
