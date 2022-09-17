using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class ProductImageUpdateViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
