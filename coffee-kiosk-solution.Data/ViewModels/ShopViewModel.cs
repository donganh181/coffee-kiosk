using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class ShopViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public Guid AreaId { get; set; }
        public string AreaName { get; set; }
        public Guid? AccountId { get; set; }
        public int Status { get; set; }
    }
}
