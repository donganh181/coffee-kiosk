﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffee_kiosk_solution.Data.Attributes.CustomAttributes;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class CampaignSearchViewModule
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever]
        public string Description { get; set; }
        public int Status { get; set; }
        [DateTime]
        public DateTime StartingDate { get; set; }
        [DateTime]
        public DateTime ExpiredDate { get; set; }
        [BindNever]
        public Guid? AreaId { get; set; }
        [String]
        public string AreaName { get; set; }


    }
}
