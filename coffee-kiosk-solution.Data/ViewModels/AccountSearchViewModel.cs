using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class AccountSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Username { get; set; }
        [BindNever]
        public string Password { get; set; }
        [Guid]
        public Guid? RoleId { get; set; }
        [String]
        public string RoleName { get; set; }
    }
}
