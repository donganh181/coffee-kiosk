using AutoMapper;
using coffee_kiosk_solution.Data.Models;
using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.AutoMapper
{
    public static class RoleModule
    {
        public static void ConfigRoleModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblRole, RoleViewModel>();
            mc.CreateMap<RoleViewModel, TblRole>();
        }
    }
}
