using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class OrderSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        public DateTime CreateDate { get; set; }
        [Guid]
        public Guid? ShopId { get; set; }
        [String]
        public string ShopName { get; set; }
        [BindNever,Skip]
        public Guid? DiscountId { get; set; }
        [BindNever,Skip]
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        [BindNever, Skip]
        public int No { get; set; }
        [BindNever,Skip]
        public double TotalPriceOfAllOrders { get; set; }
    }
}
