using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface ISupplyService
    {
        Task<SupplyViewModel> CreateNew(Guid managerId, SupplyCreateViewModel model);
        Task<SupplyViewModel> UpdateQuantity(Guid managerId, SupplyUpdateQuantityViewModel model);
        Task<SupplyViewModel> ChangeStatus(Guid managerId, Guid supplyId);
        Task<SupplyViewModel> UpdateSupplyQuantity(Guid managerId, SupplyUpdateSupQuantityViewModel model);
        Task<SupplyViewModel> StopSupply(Guid managerId, Guid supplyId);
        Task<SupplyViewModel> ReImport(Guid managerId, Guid supplyId);
        Task<DynamicModelResponse<SupplySearchViewModel>> GetAllWithPaging(Guid managerId, SupplySearchViewModel model, int size, int pageNum);
        Task<bool> CheckSupplyByWeek();
        Task<SupplyViewModel> GetById(Guid managerId, Guid supplyId);
        Task<int> GetQuantityByShopIdAndProductId(Guid shopId, Guid productId);
        Task<bool> UpdateSupplyAfterOrder(Guid shopId, Guid productId, int quantity);
        Task<DynamicModelResponse<SupplyCustomerSearchViewModel>> GetAllWithPagingByShopId(Guid shopId, SupplyCustomerSearchViewModel model, int size, int pageNum);
        Task<bool> UpdateSupplyAfterCancelOrder(Guid shopId, Guid productId, int quantity);
    }
}
