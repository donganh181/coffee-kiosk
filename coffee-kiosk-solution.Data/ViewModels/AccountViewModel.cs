using System;
using System.Collections.Generic;
using System.Text;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
        public Guid? ShopId { get; set; }
    }
}
