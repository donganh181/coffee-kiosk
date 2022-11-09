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
        public Guid ShopId { get; set; }
        public Guid? DiscountId { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        public int No { get; set; }

        public virtual TblDiscount Discount { get; set; }
        public virtual TblShop Shop { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
