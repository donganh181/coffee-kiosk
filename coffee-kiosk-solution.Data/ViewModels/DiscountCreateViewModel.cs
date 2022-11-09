using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class DiscountCreateViewModel
    {
        [Required]
        public double DiscountValue { get; set; }
        [Required]
        public Guid CampaignId { get; set; }
        [Required]
        public double RequiredValue { get; set; }

    }
}
