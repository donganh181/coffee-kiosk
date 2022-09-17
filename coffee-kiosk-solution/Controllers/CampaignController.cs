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
    [Route("api/v{version:apiVersion}/campaigns")]
    [ApiController]
    [ApiVersion("1")]
    public class CampaignController : Controller
    {
        private readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignController> _logger;
        private IConfiguration _configuration;

        public CampaignController(ICampaignService campaignService, ILogger<CampaignController> logger,
            IConfiguration configuration)
        {
            _campaignService = campaignService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow the Admin to create new campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewCampaign([FromBody] CampaignCreateViewModule model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _campaignService.Create(model);
            _logger.LogInformation($"Create campaign {result.Name} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CampaignViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow the Admin to update campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateCampaign([FromBody] CampaignUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _campaignService.Update(model);
            _logger.LogInformation($"Update campaign {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CampaignViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to change status of campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusCampaign([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _campaignService.ChangeStatus(id);
            _logger.LogInformation($"Change Status campaign {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CampaignViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Deletecampaign([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _campaignService.Delete(id);
            _logger.LogInformation($"Delete product {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CampaignViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// This feature allow user to get all campaign with paging (default value of status is 0)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllCampaignsWithPaging([FromQuery] CampaignSearchViewModule model,
            int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _campaignService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all products ");
            return Ok(new SuccessResponse<DynamicModelResponse<CampaignSearchViewModule>>
                (
                    (int)HttpStatusCode.OK, "Get success.", result)
                );
        }

    }
}
