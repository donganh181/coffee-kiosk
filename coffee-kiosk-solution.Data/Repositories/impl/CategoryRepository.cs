﻿using coffee_kiosk_solution.Data.Context;
using coffee_kiosk_solution.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.Repositories.impl
{
    public class CategoryRepository : BaseRepository<TblCategory>, ICategoryRepository
    {
        public CategoryRepository(Coffee_KioskContext dbContext) : base(dbContext)
        {

        }
    }
}
