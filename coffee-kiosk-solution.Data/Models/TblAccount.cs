using System;
using System.Collections.Generic;

#nullable disable

namespace coffee_kiosk_solution.Data.Models
{
    public partial class TblAccount
    {
        public TblAccount()
        {
            InverseCreator = new HashSet<TblAccount>();
            TblShops = new HashSet<TblShop>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public Guid? CreatorId { get; set; }

        public virtual TblAccount Creator { get; set; }
        public virtual TblRole Role { get; set; }
        public virtual ICollection<TblAccount> InverseCreator { get; set; }
        public virtual ICollection<TblShop> TblShops { get; set; }
    }
}
