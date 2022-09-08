using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblSupply
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int SupplyQuantity { get; set; }
        public int Status { get; set; }

        public virtual TblProduct Product { get; set; }
        public virtual TblShop Shop { get; set; }
    }
}
