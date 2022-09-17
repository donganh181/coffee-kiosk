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
        public float DiscountPercentage { get; set; }
        public Guid ProductId { get; set; }
        public Guid CampaignId { get; set; }
        public int Status { get; set; }
    }
}
