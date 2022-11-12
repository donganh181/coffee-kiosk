using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ShopId { get; set; }
        public Guid? DiscountId { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        public int No { get; set; }
        public List<OrderDetailViewModel> ListOrderDetail { get; set; }
    }
}
