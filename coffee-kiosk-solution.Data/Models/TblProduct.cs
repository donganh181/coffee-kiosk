using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
            TblProductImages = new HashSet<TblProductImage>();
            TblSupplies = new HashSet<TblSupply>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public virtual TblCategory Category { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual ICollection<TblProductImage> TblProductImages { get; set; }
        public virtual ICollection<TblSupply> TblSupplies { get; set; }
    }
}
