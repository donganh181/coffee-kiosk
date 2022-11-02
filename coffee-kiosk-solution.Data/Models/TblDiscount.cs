using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblDiscount
    {
        public TblDiscount()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        public Guid Id { get; set; }
        public double DiscountValue { get; set; }
        public Guid CampaignId { get; set; }
        public int Status { get; set; }
        public double RequiredValue { get; set; }
        public string Code { get; set; }

        public virtual TblCampaign Campaign { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
