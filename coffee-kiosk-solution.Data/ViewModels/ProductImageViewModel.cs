using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class ProductImageViewModel
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public Guid ProductId { get; set; }
    }
}
