using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
        }

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
