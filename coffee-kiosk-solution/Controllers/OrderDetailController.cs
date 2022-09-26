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
    [Route("api/v{version:apiVersion}/orderDetails")]
    [ApiController]
    [ApiVersion("1")]
    public class OrderDetailController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(ILogger<OrderController> logger, IConfiguration configuration, IOrderDetailService orderDetailService)
        {
            _logger = logger;
            _configuration = configuration;
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// This function allow the user to create a new Order Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<ActionResult> CreateNewOrderDetail([FromBody] OrderDetailCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _orderDetailService.Create(model);
            _logger.LogInformation($"Create OrderDetail for Order: {model.OrderId}");
            return Ok(new SuccessResponse<OrderDetailViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }


        /// <summary>
        /// This function allow user to get Order Detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> GetOrderDetailById(Guid id)
        {
            var result = await _orderDetailService.GetById(id);
            _logger.LogInformation($"Get OrderDetail {result.Id}");
            return Ok(new SuccessResponse<OrderDetailViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow user to get all order detail with paging
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<ActionResult> GetAllOrderDetailsWithPaging([FromQuery] OrderDetailSearchViewModel model,
            int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _orderDetailService.GetAllWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get all order detail ");
            return Ok(new SuccessResponse<DynamicModelResponse<OrderDetailSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }


        /// <summary>
        /// This feature allow user to get all order detail with orderId with paging
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> GetAllOrderDetailsByOrderIdWithPaging([FromQuery] Guid orderId, OrderDetailSearchViewModel model,
            int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _orderDetailService.GetAllByOrderId(orderId, model, size, pageNum);
            _logger.LogInformation($"Get all order detail ");
            return Ok(new SuccessResponse<DynamicModelResponse<OrderDetailSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow user to get all order detail with shopId with paging
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet("{shopId}")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> GetAllOrderDetailsByShopIdWithPaging([FromQuery] Guid shopId, OrderDetailSearchViewModel model,
            int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _orderDetailService.GetAllByShopId(shopId, model, size, pageNum);
            _logger.LogInformation($"Get all order detail ");
            return Ok(new SuccessResponse<DynamicModelResponse<OrderDetailSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }

    }
}
