using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IProductService
    {
        Task<ProductViewModel> Create(ProductCreateViewModel model);
        Task<ProductViewModel> Update(ProductUpdateViewModel model);
        Task<ProductViewModel> ChangeStatus(Guid id);
        Task<ProductViewModel> Delete(Guid id);
        Task<ProductViewModel> GetById(Guid id);
        Task<DynamicModelResponse<ProductSearchViewModel>> GetAllWithPaging(ProductSearchViewModel model, int size, int pageNum);
    }
}
