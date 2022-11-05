using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace coffee_kiosk_solution.Business.Services
{
    public interface IOrderService
    {
        Task<OrderViewModel> Create(OrderCreateViewModel model);
        Task<OrderViewModel> GetById(Guid id);
        Task<DynamicModelResponse<OrderSearchViewModel>> GetAllWithPaging(OrderSearchViewModel model, int size, int pageNum);
        Task<OrderViewModel> ChangeStatus(Guid id, Guid shopId);
    }
}
