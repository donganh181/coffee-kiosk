using coffee_kiosk_solution.Business.Services;
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
    [Route("api/v{version:apiVersion}/roles")]
    [ApiController]
    [ApiVersion("1")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        private IConfiguration _configuration;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger,
            IConfiguration configuration)
        {
            _roleService = roleService;
            _logger = logger;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllRoles()
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _roleService.GetListRole();
            _logger.LogInformation($"Get all roles by admin with Id {token.Id} ");
            return Ok(new SuccessResponse<List<RoleViewModel>>
                (
                    (int)HttpStatusCode.OK, "Get success.", result)
                );
        }
    }
}
