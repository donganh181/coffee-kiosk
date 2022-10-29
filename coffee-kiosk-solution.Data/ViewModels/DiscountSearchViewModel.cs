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
        [BindNever, Skip]
        public double DiscountPercentage { get; set; }
        [BindNever]
        public Guid? ProductId { get; set; }
        [String]
        public string ProductName { get; set; }
        [BindNever]
        public Guid? CampaignId { get; set; }
        [String]
        public string CampaignName { get; set; }
        public int Status { get; set; }
    }
}
