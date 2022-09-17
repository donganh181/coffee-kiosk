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
    [Route("api/v{version:apiVersion}/areas")]
    [ApiController]
    [ApiVersion("1")]
    public class AreaController : Controller
    {
        private readonly IAreaService _areaService;
        private readonly ILogger<AreaController> _logger;
        private IConfiguration _configuration;

        public AreaController(IAreaService areaService, ILogger<AreaController> logger, IConfiguration configuration)
        {
            _areaService = areaService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow the Admin to create new area
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewDiscount([FromBody] AreaCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _areaService.Create(model);
            _logger.LogInformation($"Create Discout {result.AreaName} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<AreaViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow the Admin to update area
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateArea([FromBody] AreaUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _areaService.Update(model);
            _logger.LogInformation($"Update area {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<AreaViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete area
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteArea([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _areaService.Delete(id);
            _logger.LogInformation($"Delete area {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<AreaViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// This feature allow user to get all area with paging (default value of status is 0)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllAreasWithPaging([FromQuery] AreaSearchViewModel model,
            int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _areaService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all products ");
            return Ok(new SuccessResponse<DynamicModelResponse<AreaSearchViewModel>>
                (
                    (int)HttpStatusCode.OK, "Get success.", result)
                );
        }

        /// <summary>
        /// This feature allow user to get area by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAreaById(Guid id)
        {
            var result = await _areaService.GetById(id);
            _logger.LogInformation($"Get product {result.Id}");
            return Ok(new SuccessResponse<AreaViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

    }
}
