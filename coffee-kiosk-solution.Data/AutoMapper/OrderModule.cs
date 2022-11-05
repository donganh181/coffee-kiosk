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
    public static class OrderModule
    {
        public static void ConfigOrderModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblOrder, OrderViewModel>();
            mc.CreateMap<OrderViewModel, TblOrder>();

            mc.CreateMap<TblOrder, OrderCreateViewModel>();
            mc.CreateMap<OrderCreateViewModel, TblOrder>();

            mc.CreateMap<TblOrder, OrderSearchViewModel>();
            mc.CreateMap<OrderSearchViewModel, TblOrder>();
        }
    }
}
