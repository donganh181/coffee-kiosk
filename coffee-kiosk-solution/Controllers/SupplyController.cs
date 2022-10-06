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
    [Route("api/v{version:apiVersion}/supplies")]
    [ApiController]
    [ApiVersion("1")]
    public class SupplyController : Controller
    {
        private readonly ISupplyService _supplyService;
        private readonly ILogger<SupplyController> _logger;
        private IConfiguration _configuration;
        public SupplyController(ISupplyService supplyService, ILogger<SupplyController> logger,
           IConfiguration configuration)
        {
            _supplyService = supplyService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow staff to add new supply to their shop
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddNewSupply([FromBody] SupplyCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.CreateNew(token.Id,model);
            _logger.LogInformation($"add new supply {result.ProductName} by staff with id: {token.Id}");
            return Ok(new SuccessResponse<SupplyViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow staff to update quantity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpPatch("quantity")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateQuantity([FromBody] SupplyUpdateQuantityViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.UpdateQuantity(token.Id,model);
            _logger.LogInformation($"Update quantity to supply {result.ProductName} by staff with id: {token.Id}");
            return Ok(new SuccessResponse<SupplyViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow staff to update supply quantity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpPatch("supply-quantity")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateSupplyQuantity([FromBody] SupplyUpdateSupQuantityViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.UpdateSupplyQuantity(token.Id,model);
            _logger.LogInformation($"Update supply quantity {result.ProductName} by staff with id: {token.Id}");
            return Ok(new SuccessResponse<SupplyViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow staff to change status of supply (available/unavailable)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusSupply([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.ChangeStatus(token.Id, id);
            _logger.LogInformation($"Change Status supply {result.ProductName} by staff with id: {token.Id}");
            return Ok(new SuccessResponse<SupplyViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow staff to stop getting supply by week 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpPatch("stop")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> StopSupply([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.StopSupply(token.Id,id);
            _logger.LogInformation($"Stop getting supply of product {result.ProductName} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<SupplyViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow staff to get all supplies with paging
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllSuppliesWithPaging([FromQuery] SupplySearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.GetAllWithPaging(token.Id, model, size, pageNum);
            _logger.LogInformation($"Get all products ");
            return Ok(new SuccessResponse<DynamicModelResponse<SupplySearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow staff to get supply by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetSupplyById(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _supplyService.GetById(token.Id, id);
            _logger.LogInformation($"Get shop {result.Id}");
            return Ok(new SuccessResponse<SupplyViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
