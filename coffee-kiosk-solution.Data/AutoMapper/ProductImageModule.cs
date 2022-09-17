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
    public static class ProductImageModule
    {
        public static void ConfigProductImageModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblProductImage, ProductImageViewModel>();
            mc.CreateMap<ProductImageViewModel, TblProductImage>();

            mc.CreateMap<TblProductImage, ProductImageCreateViewModel>();
            mc.CreateMap<ProductImageCreateViewModel, TblProductImage>();

            mc.CreateMap<TblProductImage, ProductImageUpdateViewModel>();
            mc.CreateMap<ProductImageUpdateViewModel, TblProductImage>();

            mc.CreateMap<TblProductImage, ProductImageDetailViewModel>()
                .ForMember(src => src.ProductName, opt => opt.MapFrom(des => des.Product.Name))
                .ForMember(src => src.CategoryName, opt => opt.MapFrom(des => des.Product.Category.Name));
            mc.CreateMap<ProductImageDetailViewModel, TblProductImage>();
        }
    }
}
