using coffee_kiosk_solution.Business.Services;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthenController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AuthenController> _logger;
        private IConfiguration _configuration;
        public AuthenController(IAccountService accountService, ILogger<AuthenController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _accountService = accountService;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow unauthenticated user to login to system
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            var result = await _accountService.Login(request);
            _logger.LogInformation($"Login by {request.Username}");
            return Ok(new SuccessResponse<AccountViewModel>(200, "Login Success.", result));
        }

    }
}
