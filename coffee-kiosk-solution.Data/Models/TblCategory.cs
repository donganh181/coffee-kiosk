using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
