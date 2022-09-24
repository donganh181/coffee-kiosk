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
    [Route("api/v{version:apiVersion}/shops")]
    [ApiController]
    [ApiVersion("1")]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly ILogger<ShopController> _logger;
        private IConfiguration _configuration;

        public ShopController(IShopService shopService, ILogger<ShopController> logger
            ,IConfiguration configuration)
        {
            _shopService = shopService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow admin to create new shop
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewShop([FromBody] ShopCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _shopService.Create(model);
            _logger.LogInformation($"Create shop {result.Name} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ShopViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow admin to update information of shop
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateShop([FromBody] ShopUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _shopService.Update(model);
            _logger.LogInformation($"Update shop {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ShopViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to change status of shop
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusShop([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _shopService.ChangeStatus(id);
            _logger.LogInformation($"Change Status Shop {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ShopViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete shop
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteShop([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _shopService.Delete(id);
            _logger.LogInformation($"Delete product {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<ShopViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// This feature allow authenticated user to get shop by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetShopById(Guid id)
        {
            var result = await _shopService.GetById(id);
            _logger.LogInformation($"Get shop {result.Id}");
            return Ok(new SuccessResponse<ShopViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow authenticated user to get all shops with paging (default value of status is 0)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Staff")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllProductsWithPaging([FromQuery] ShopSearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _shopService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all shops ");
            return Ok(new SuccessResponse<DynamicModelResponse<ShopSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
