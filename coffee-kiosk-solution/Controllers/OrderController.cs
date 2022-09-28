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
    [Route("api/v{version:apiVersion}/orders")]
    [ApiController]
    [ApiVersion("1")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IConfiguration configuration,
            IOrderService orderService)
        {
            _logger = logger;
            _configuration = configuration;
            _orderService = orderService;
        }

        /// <summary>
        /// this feature is use to create new order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewOrder([FromBody] OrderCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _orderService.Create(model);
            _logger.LogInformation($"Create order {result.Id}");
            return Ok(new SuccessResponse<OrderViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow user to get order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetOrderByID(Guid id)
        {
            var result = await _orderService.GetById(id);
            _logger.LogInformation($"Get order {result.Id}");
            return Ok(new SuccessResponse<OrderViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow user to get all order with paging
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllOrderWithPaging([FromQuery] OrderSearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _orderService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all products ");
            return Ok(new SuccessResponse<DynamicModelResponse<OrderSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
