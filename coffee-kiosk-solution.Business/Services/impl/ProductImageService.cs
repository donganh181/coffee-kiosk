using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ProductImageService : IProductImageService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IProductImageService> _logger;
        private readonly IFileService _fileService;

        public ProductImageService(IMapper mapper, IConfiguration configuration,
            IUnitOfWork unitOfWork, ILogger<IProductImageService> logger,
            IFileService fileService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<ProductImageViewModel> Create(ProductImageCreateViewModel model)
        {
            var image = _mapper.Map<TblProductImage>(model);
            try
            {
                await _unitOfWork.ProductImageRepository.InsertAsync(image);
                await _unitOfWork.SaveAsync();
                var img = await _unitOfWork.ProductImageRepository
                    .Get(i => i.Id.Equals(image.Id))
                    .Include(a => a.Product)
                    .ThenInclude(b => b.Category)
                    .ProjectTo<ProductImageDetailViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                var link = await _fileService.UploadImageToFirebase(model.Image, img.CategoryName, img.Id, img.ProductName);
                image.Link = link;

                _unitOfWork.ProductImageRepository.Update(image);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<ProductImageViewModel>(image);
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var image = await _unitOfWork.ProductImageRepository
                .Get(i => i.Id.Equals(id))
                .FirstOrDefaultAsync();
            if (image == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            var listImage = await _unitOfWork.ProductImageRepository
                .Get(i => i.ProductId.Equals(image.ProductId))
                .ProjectTo<ProductImageViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (listImage.Count == 1)
            {
                _logger.LogError("Cannot delete last image in this product.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cannot delete last image in this product.");
            }
            try
            {
                _unitOfWork.ProductImageRepository.Delete(image);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<ProductImageViewModel> GetById(Guid id)
        {
            var image = await _unitOfWork.ProductImageRepository
                .Get(i => i.Id.Equals(id))
                .ProjectTo<ProductImageViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (image == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return image;
        }

        public async Task<List<ProductImageViewModel>> GetListImageByProductId(Guid productId)
        {
            var listImage = await _unitOfWork.ProductImageRepository
                .Get(i => i.ProductId.Equals(productId))
                .ProjectTo<ProductImageViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (listImage.Count<1)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return listImage;
        }

        public async Task<ProductImageViewModel> Update(ProductImageUpdateViewModel model)
        {
            var image = await _unitOfWork.ProductImageRepository
                    .Get(i => i.Id.Equals(model.Id))
                    .Include(a => a.Product)
                    .ThenInclude(b => b.Category)
                    .ProjectTo<ProductImageDetailViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            if (image == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            try
            {
                var link = await _fileService.UploadImageToFirebase(model.Image, image.CategoryName, image.Id, image.ProductName);

                var updateImage = await _unitOfWork.ProductImageRepository
                    .Get(i => i.Id.Equals(model.Id))
                    .FirstOrDefaultAsync();

                updateImage.Link = link;

                _unitOfWork.ProductImageRepository.Update(updateImage);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<ProductImageViewModel>(updateImage);
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }
    }
}
