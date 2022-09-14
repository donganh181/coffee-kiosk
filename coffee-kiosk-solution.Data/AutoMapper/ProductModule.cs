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
    public static class ProductModule
    {
        public static void ConfigProductModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblProduct, ProductViewModel>()
                .ForMember(src => src.CategoryName, opt => opt.MapFrom(des => des.Category.Name));
            mc.CreateMap<ProductViewModel, TblProduct>();

            mc.CreateMap<TblProduct, ProductCreateViewModel>();
            mc.CreateMap<ProductCreateViewModel, TblProduct>();

            mc.CreateMap<TblProduct, ProductUpdateViewModel>();
            mc.CreateMap<ProductUpdateViewModel, TblProduct>();

            mc.CreateMap<TblProduct, ProductSearchViewModel>()
                .ForMember(src => src.CategoryName, opt => opt.MapFrom(des => des.Category.Name));
            mc.CreateMap<ProductSearchViewModel, TblProduct>();
        }
    }
}
