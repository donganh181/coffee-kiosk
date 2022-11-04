using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> Create(DiscountCreateViewModel discountViewModel);
        Task<DiscountViewModel> Update(DiscountUpdateViewModel discountViewModel);
        Task<DiscountViewModel> ChangeStatus(Guid id);
        Task<DiscountViewModel> Delete(Guid id);
        Task<DiscountViewModel> GetById(Guid id);
        Task<DynamicModelResponse<DiscountSearchViewModel>> GetAllWithPaging(DiscountSearchViewModel model, int size, int pageNum);
        Task<List<DiscountViewModel>> GetListDiscountByCampaign(Guid campaignId);
        Task<DiscountViewModel> CheckDiscountByShopId(Guid discountId, Guid shopId);
    }
}
