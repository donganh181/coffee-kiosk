using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IShopService
    {
        Task<ShopViewModel> Create(ShopCreateViewModel model);
        Task<ShopViewModel> Update(ShopUpdateViewModel model);
        Task<ShopViewModel> ChangeStatus(Guid id);
        Task<ShopViewModel> Delete(Guid id);
        Task<ShopViewModel> ChangeShopManager(Guid shopId, Guid? managerId);
        Task<DynamicModelResponse<ShopSearchViewModel>> GetAllWithPaging(ShopSearchViewModel model, int size, int pageNum, string role, Guid managerId);
        Task<ShopViewModel> GetById(Guid id, string role, Guid managerId);
    }
}
