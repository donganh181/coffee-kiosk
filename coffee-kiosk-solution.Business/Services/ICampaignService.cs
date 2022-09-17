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
        Task<CampaignViewModel> Create(CampaignCreateViewModule model);
        Task<CampaignViewModel> Update(CampaignUpdateViewModel model);
        Task<CampaignViewModel> ChangeStatus(Guid id);
        Task<CampaignViewModel> Delete(Guid id);
        Task<CampaignViewModel> GetById(Guid id);
        Task<DynamicModelResponse<CampaignSearchViewModule>> GetAllWithPaging(CampaignSearchViewModule model, int size, int pageNum);
    }
}
