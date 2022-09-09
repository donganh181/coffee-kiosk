﻿using coffee_kiosk_solution.Data.Context;
using coffee_kiosk_solution.Data.Repositories;
using coffee_kiosk_solution.Data.Repositories.impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace coffee_kiosk_solution.Business.DI
{
    public static class ServicesDI
    {
        public static void ConfigServicesDI(this IServiceCollection services)
        {
            services.AddScoped<DbContext, Coffee_KioskContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}