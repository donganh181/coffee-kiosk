using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblCampaign
    {
        public TblCampaign()
        {
            TblDiscounts = new HashSet<TblDiscount>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public Guid AreaId { get; set; }

        public virtual TblArea Area { get; set; }
        public virtual ICollection<TblDiscount> TblDiscounts { get; set; }
    }
}
