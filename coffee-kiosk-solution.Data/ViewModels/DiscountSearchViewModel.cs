using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class DiscountSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        public double DiscountPercentage { get; set; }
        [BindNever]
        public Guid? ProductId { get; set; }
        [BindNever]
        public Guid? CampaignId { get; set; }
        public int Status { get; set; }
    }
}
