using coffee_kiosk_solution.Business.Services;
using coffee_kiosk_solution.Data.Constants;
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
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    [ApiVersion("1")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private IConfiguration _configuration;

        public ProductController(IProductService productService, ILogger<ProductController> logger,
            IConfiguration configuration)
        {
            _productService = productService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow admin to create new product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewProduct([FromBody] ProductCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _productService.Create(model);
            _logger.LogInformation($"Create product {result.Name} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ProductViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow admin to update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _productService.Update(model);
            _logger.LogInformation($"Update product {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ProductViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to change status of product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusProduct([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _productService.ChangeStatus(id);
            _logger.LogInformation($"Change Status product {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ProductViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete product
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
            var result = await _productService.Delete(id);
            _logger.LogInformation($"Delete product {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ProductViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// This feature allow user to get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _productService.GetById(id);
            _logger.LogInformation($"Get product {result.Id}");
            return Ok(new SuccessResponse<ProductViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow user to get all product with paging (default value of status is 0)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllProductsWithPaging([FromQuery] ProductSearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _productService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all products ");
            return Ok(new SuccessResponse<DynamicModelResponse<ProductSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
