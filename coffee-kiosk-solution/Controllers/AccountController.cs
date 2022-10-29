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
using System.Net;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    [ApiVersion("1")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        private IConfiguration _configuration;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger,
            IConfiguration configuration)
        {
            _accountService = accountService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow admin to Create new account to staff
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Register([FromBody] AccountCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid creatorId = token.Id;
            var result = await _accountService.Create(creatorId, model);
            _logger.LogInformation($"Created account {result.Username} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<AccountViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllAccountWithPaging([FromQuery] AccountSearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _accountService.GetListAccount(model, size, pageNum);
            _logger.LogInformation($"Get all account by admin with id {token.Id} ");
            return Ok(new SuccessResponse<DynamicModelResponse<AccountSearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _accountService.GetById(id, token.Role, token.Id);
            _logger.LogInformation($"Get account by user with id {token.Id} ");
            return Ok(new SuccessResponse<AccountViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
