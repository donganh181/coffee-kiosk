using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class SupplySearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [Guid]
        public Guid? ShopId { get; set; }
        [String]
        public string ShopName { get; set; }
        [Guid]
        public Guid? ProductId { get; set; }
        [String]
        public string ProductName { get; set; }
        [BindNever,Skip]
        public List<ProductImageViewModel> ListImage { get; set; }
        [BindNever, Skip]
        public int Quantity { get; set; }
        [BindNever, Skip]
        public int SupplyQuantity { get; set; }
        public int Status { get; set; }
    }
}
