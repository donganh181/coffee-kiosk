using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class OrderDetailSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [BindNever]
        public Guid? ProductId { get; set; }
        [BindNever, Skip]
        public int Quantity { get; set; }
        [BindNever, Skip]
        public double Price { get; set; }
        [BindNever]
        public Guid? OrderId { get; set; }
        [BindNever]
        public Guid? ShopId { get; set; }
    }
}
