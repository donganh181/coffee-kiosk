using System;
using System.Collections.Generic;
using System.Text;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class TokenViewModel
    {
        public TokenViewModel(Guid id, string role)
        {
            Id = id;
            Role = role;
        }
        public Guid Id { get; set; }
        public string Role { get; set; }
    }
}
