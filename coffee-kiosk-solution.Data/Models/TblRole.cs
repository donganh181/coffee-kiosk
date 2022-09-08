using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblAccounts = new HashSet<TblAccount>();
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<TblAccount> TblAccounts { get; set; }
    }
}
