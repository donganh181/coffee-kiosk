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
    public static class AreaModule
    {
        public static void ConfigAreaModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblArea, AreaViewModel>();
            mc.CreateMap<AreaViewModel, TblArea>();

            mc.CreateMap<TblArea, AreaCreateViewModel>();
            mc.CreateMap<AreaCreateViewModel, TblArea>();

            mc.CreateMap<TblArea, AreaSearchViewModel>();
            mc.CreateMap<AreaSearchViewModel, TblArea>();

            mc.CreateMap<TblArea, AreaUpdateViewModel>();
            mc.CreateMap<AreaUpdateViewModel, TblArea>();
        }
    }
}
