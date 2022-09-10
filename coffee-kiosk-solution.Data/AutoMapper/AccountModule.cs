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
    public static class AccountModule
    {
        public static void ConfigAccountModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblAccount, AccountViewModel>()
                .ForMember(src => src.RoleName, opt => opt.MapFrom(des => des.Role.RoleName));
            mc.CreateMap<AccountViewModel, TblAccount>();

            mc.CreateMap<TblAccount, AccountCreateViewModel>();
            mc.CreateMap<AccountCreateViewModel, TblAccount>();
        }
    }
}
