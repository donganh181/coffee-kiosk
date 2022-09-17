using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class CampaignCreateViewModule
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        [Required]
        public DateTime ExpiredDate { get; set; }
        [Required]
        public Guid AreaId { get; set; }
    }
}
