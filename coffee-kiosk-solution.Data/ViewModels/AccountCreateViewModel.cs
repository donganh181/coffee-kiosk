using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class AccountCreateViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }
}
