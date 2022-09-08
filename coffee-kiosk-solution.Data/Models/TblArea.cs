using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblArea
    {
        public TblArea()
        {
            TblCampaigns = new HashSet<TblCampaign>();
            TblShops = new HashSet<TblShop>();
        }

        public Guid Id { get; set; }
        public string AreaName { get; set; }

        public virtual ICollection<TblCampaign> TblCampaigns { get; set; }
        public virtual ICollection<TblShop> TblShops { get; set; }
    }
}
