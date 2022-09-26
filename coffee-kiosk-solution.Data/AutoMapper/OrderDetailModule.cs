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
    public static class OrderDetailModule
    {
        public static void ConfigOrderDetailModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblOrderDetail, OrderDetailViewModel>();
            mc.CreateMap<OrderDetailViewModel, TblOrderDetail>();

            mc.CreateMap<TblOrderDetail, OrderDetailCreateViewModel>();
            mc.CreateMap<OrderDetailCreateViewModel, TblOrderDetail>();

            mc.CreateMap<TblOrderDetail, OrderDetailSearchViewModel>();
            mc.CreateMap<OrderDetailSearchViewModel, TblOrderDetail>();
        }
    }
}
