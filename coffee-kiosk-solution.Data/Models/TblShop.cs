using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblShop
    {
        public TblShop()
        {
            TblOrders = new HashSet<TblOrder>();
            TblSupplies = new HashSet<TblSupply>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public Guid AreaId { get; set; }
        public Guid? AccountId { get; set; }
        public int Status { get; set; }

        public virtual TblAccount Account { get; set; }
        public virtual TblArea Area { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblSupply> TblSupplies { get; set; }
    }
}
