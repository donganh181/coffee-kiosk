using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
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
        [BindNever, Skip]
        public DateTime CreateDate { get; set; }
    }
}
