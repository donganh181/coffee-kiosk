using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IAreaService
    {
        Task<AreaViewModel> Create(AreaCreateViewModel model);
        Task<AreaViewModel> Update(AreaUpdateViewModel model);
        Task<AreaViewModel> Delete(Guid id);
        Task<AreaViewModel> GetById(Guid id);
        Task<DynamicModelResponse<AreaSearchViewModel>> GetAllWithPaging(AreaSearchViewModel model, int size, int pageNum);
    }
}
