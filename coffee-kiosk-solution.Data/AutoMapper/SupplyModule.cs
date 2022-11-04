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
    public static class SupplyModule
    {
        public static void ConfigSupplyModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblSupply, SupplyViewModel>()
                .ForMember(src => src.ProductName, opt => opt.MapFrom(des => des.Product.Name))
                .ForMember(src => src.ListImage, opt => opt.MapFrom(des => des.Product.TblProductImages.ToList()))
                .ForMember(src => src.ShopName, opt => opt.MapFrom(des => des.Shop.Name));
            mc.CreateMap<SupplyViewModel, TblSupply>();

            mc.CreateMap<TblSupply, SupplyCreateViewModel>();
            mc.CreateMap<SupplyCreateViewModel, TblSupply>();

            mc.CreateMap<TblSupply, SupplySearchViewModel>()
                .ForMember(src => src.ProductName, opt => opt.MapFrom(des => des.Product.Name))
                .ForMember(src => src.ListImage, opt => opt.MapFrom(des => des.Product.TblProductImages.ToList()))
                .ForMember(src => src.ShopName, opt => opt.MapFrom(des => des.Shop.Name));
            mc.CreateMap<SupplySearchViewModel, TblSupply>();

            mc.CreateMap<TblSupply, SupplyCustomerSearchViewModel>()
                .ForMember(src => src.ProductName, opt => opt.MapFrom(des => des.Product.Name))
                .ForMember(src => src.ListImage, opt => opt.MapFrom(des => des.Product.TblProductImages.ToList()))
                .ForMember(src => src.ShopName, opt => opt.MapFrom(des => des.Shop.Name))
                .ForMember(src => src.CategoryId, opt => opt.MapFrom(des => des.Product.Category.Id))
                .ForMember(src => src.CategoryName, opt => opt.MapFrom(des => des.Product.Category.Name))
                .ForMember(src => src.Description, opt => opt.MapFrom(des => des.Product.Description))
                .ForMember(src => src.Price, opt => opt.MapFrom(des => des.Product.Price));
            mc.CreateMap<SupplyCustomerSearchViewModel, TblSupply>();
        }
    }
}
