using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> Create(CategoryCreateViewModel model);
        Task<CategoryViewModel> Update(CategoryUpdateViewModel model);
        Task<CategoryViewModel> ChangeStatus(Guid id);
        Task<CategoryViewModel> Delete(Guid id);
        Task<CategoryViewModel> GetById(Guid id);
        Task<DynamicModelResponse<CategorySearchViewModel>> GetAllWithPaging(CategorySearchViewModel model, int size, int pageNum);
    }
}
