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
    [Route("api/v{version:apiVersion}/discounts")]
    [ApiController]
    [ApiVersion("1")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountController> _logger;
        private IConfiguration _configuration;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger, IConfiguration configuration)
        {
            _discountService = discountService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow the Admin to create new discount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewDiscount([FromBody] DiscountCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _discountService.Create(model);
            _logger.LogInformation($"Create discount {result.DiscountPercentage} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<DiscountViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow the Admin to update discount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _discountService.Update(model);
            _logger.LogInformation($"Update discount {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<DiscountViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to change status of discount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusDiscount([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _discountService.ChangeStatus(id);
            _logger.LogInformation($"Change Status discount {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<DiscountViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete discount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteDiscount([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _discountService.Delete(id);
            _logger.LogInformation($"Delete area {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<DiscountViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// This feature allow user to get all discounts with paging (default value of status is 0)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllDiscountsWithPaging([FromQuery] DiscountSearchViewModel model,
            int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _discountService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all products ");
            return Ok(new SuccessResponse<DynamicModelResponse<DiscountSearchViewModel>>
                (
                    (int)HttpStatusCode.OK, "Get success.", result)
                );
        }

        /// <summary>
        /// This feature allow user to get discount by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetDiscountById(Guid id)
        {
            var result = await _discountService.GetById(id);
            _logger.LogInformation($"Get product {result.Id}");
            return Ok(new SuccessResponse<DiscountViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

    }
}
