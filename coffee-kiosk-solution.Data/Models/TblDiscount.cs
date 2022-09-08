using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblDiscount
    {
        public Guid Id { get; set; }
        public double DiscountPercentage { get; set; }
        public Guid ProductId { get; set; }
        public Guid CampaignId { get; set; }
        public int Status { get; set; }

        public virtual TblCampaign Campaign { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
