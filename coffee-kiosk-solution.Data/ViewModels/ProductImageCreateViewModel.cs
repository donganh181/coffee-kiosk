using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class ProductImageCreateViewModel
    {
        [Required]
        public string Image { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}
