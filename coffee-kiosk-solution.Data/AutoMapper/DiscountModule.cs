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
    public static class DiscountModule
    {
        public static void ConfigDiscountModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblDiscount, DiscountViewModel>();
            mc.CreateMap<DiscountViewModel, TblDiscount>();

            mc.CreateMap<TblDiscount, DiscountCreateViewModel>();
            mc.CreateMap<DiscountCreateViewModel, TblDiscount>();

            mc.CreateMap<TblDiscount, DiscountUpdateViewModel>();
            mc.CreateMap<DiscountUpdateViewModel, TblDiscount>();

            mc.CreateMap<TblDiscount, DiscountSearchViewModel>();
            mc.CreateMap<DiscountSearchViewModel, TblDiscount>();
        }
    }
}
