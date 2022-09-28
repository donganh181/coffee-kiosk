using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
