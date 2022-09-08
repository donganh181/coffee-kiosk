using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblProductImage
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public Guid ProductId { get; set; }

        public virtual TblProduct Product { get; set; }
    }
}
