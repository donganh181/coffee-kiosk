using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;

namespace coffee_kiosk_solution.Business.Services
{
    public interface ICampaignService
    {
        Task<CampaignViewModel> Create(CampaignCreateViewModle model);
        Task<CampaignViewModel> Update(CampaignUpdateViewModel model);
        Task<CampaignViewModel> ChangeStatus(Guid id);
        Task<CampaignViewModel> Delete(Guid id);
        Task<CampaignViewModel> GetById(Guid id);
        Task<DynamicModelResponse<CampaignSearchViewModel>> GetAllWithPaging(CampaignSearchViewModel model, int size, int pageNum);
    }
}
