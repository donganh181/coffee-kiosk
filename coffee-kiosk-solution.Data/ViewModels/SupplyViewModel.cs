using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class SupplyViewModel
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public List<ProductImageViewModel> ListImage { get; set; }
        public int Quantity { get; set; }
        public int SupplyQuantity { get; set; }
        public int Status { get; set; }
    }
}
