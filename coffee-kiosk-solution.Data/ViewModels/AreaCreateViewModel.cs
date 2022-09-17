using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class AreaCreateViewModel
    {
        [Required]
        public string AreaName { get; set; }
    }
}
