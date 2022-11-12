using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class DiscountViewModel
    {
        public Guid Id { get; set; }
        public double DiscountValue { get; set; }
        public Guid CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int Status { get; set; }
        public double RequiredValue { get; set; }
    }
}
