using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        public List<OrderSpecificCreateViewModel> ListOrder { get; set; }
        [Required]
        public Guid ShopId { get; set; }
        public Guid? DiscountId { get; set; }
        [Required]
        public double TotalPriceBeforeDiscount { get; set; }
    }
}
