using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace coffee_kiosk_solution.Business.Services
{
    public interface IOrderDetailService
    {
        Task<OrderDetailViewModel> Create(OrderDetailCreateViewModel model);
        Task<OrderDetailViewModel> GetById(Guid id);
        Task<DynamicModelResponse<OrderDetailSearchViewModel>> GetAllWithPaging(OrderDetailSearchViewModel model, int size, int pageNum);
    }
}
