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
    [Route("api/v{version:apiVersion}/category")]
    [ApiController]
    [ApiVersion("1")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private IConfiguration _configuration;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger,
            IConfiguration configuration)
        {
            _categoryService = categoryService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This feature allow admin to create new category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewCategory([FromBody] CategoryCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _categoryService.Create(model);
            _logger.LogInformation($"Created category {result.Name} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CategoryViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// This feature allow admin to update information to category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _categoryService.Update(model);
            _logger.LogInformation($"Update category {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CategoryViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to activate/deactivate category in system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusCategory([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _categoryService.ChangeStatus(id);
            _logger.LogInformation($"Change Status category {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CategoryViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// This feature allow admin to delete category in system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteCategory([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _categoryService.Delete(id);
            _logger.LogInformation($"Change Status category {result.Id} by admin with id: {token.Id}");
            return Ok(new SuccessResponse<CategoryViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// This feature allow user to get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetById(id);
            _logger.LogInformation($"Get category {result.Id}");
            return Ok(new SuccessResponse<CategoryViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }

        /// <summary>
        /// This feature allow user to get table of category
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] CategorySearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var result = await _categoryService.GetAllWithPaging(model,size,pageNum);
            _logger.LogInformation($"Get all category ");
            return Ok(new SuccessResponse<DynamicModelResponse<CategorySearchViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
