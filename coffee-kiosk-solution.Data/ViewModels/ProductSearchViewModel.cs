using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class ProductSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int Status { get; set; }
        [BindNever]
        public Guid? CategoryId { get; set; }
        [String]
        public string CategoryName { get; set; }
        [BindNever]
        public string Description { get; set; }
        [Skip, BindNever]
        public double Price { get; set; }
        [Skip, BindNever]
        public List<ProductImageViewModel> ListImage { get; set; }
    }
}
