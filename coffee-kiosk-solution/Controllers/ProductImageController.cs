using coffee_kiosk_solution.Business.Services;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using coffee_kiosk_solution.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/productImages")]
    [ApiController]
    [ApiVersion("1")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;
        private readonly ILogger<ProductImageController> _logger;
        private IConfiguration _configuration;

        public ProductImageController(IProductImageService productImageService, ILogger<ProductImageController> logger,
            IConfiguration configuration)
        {
            _productImageService = productImageService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow admin to add image to product by admin with base-64 image and image id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddImageToProduct([FromBody] ProductImageCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _productImageService.Create(model);
            _logger.LogInformation($"Add image to product Id '{result.Id}' by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ProductImageViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow admin to change image from product by admin with base-64 image and image id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeImageFromProduct([FromBody] ProductImageUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _productImageService.Update(model);
            _logger.LogInformation($"Update image Id '{result.Id}' by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ProductImageViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete image from product if only there were more than 1 image in that product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteProduct([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _productImageService.Delete(id);
            _logger.LogInformation($"Delete image id {id} by admin with id: {token.Id}");
            return Ok(result);
        }

        /// <summary>
        /// This feature allow user to get product image by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProductImageById(Guid id)
        {
            var result = await _productImageService.GetById(id);
            _logger.LogInformation($"Get image {result.Id} from product id'{result.ProductId}'");
            return Ok(new SuccessResponse<ProductImageViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
