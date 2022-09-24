using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class ShopSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever, Skip]
        public string Description { get; set; }
        [String]
        public string Address { get; set; }
        [Guid]
        public Guid? AreaId { get; set; }
        [String]
        public string AreaName { get; set; }
        [Guid]
        public Guid? AccountId { get; set; }
        [String]
        public string Username { get; set; }
        public int Status { get; set; }
    }
}
