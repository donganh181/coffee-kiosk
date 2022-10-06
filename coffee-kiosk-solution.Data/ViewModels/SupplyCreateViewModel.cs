using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class SupplyCreateViewModel
    {
        public Guid ShopId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
