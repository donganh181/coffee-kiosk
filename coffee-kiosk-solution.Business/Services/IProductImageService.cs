using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IProductImageService
    {
        Task<ProductImageViewModel> Create(ProductImageCreateViewModel model);
        Task<ProductImageViewModel> Update(ProductImageUpdateViewModel model);
        Task<bool> Delete(Guid id);
        Task<ProductImageViewModel> GetById(Guid id);
        Task<List<ProductImageViewModel>> GetListImageByProductId(Guid productId);
    }
}
