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
    public static class ShopModule
    {
        public static void ConfigShopModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblShop, ShopViewModel>()
                .ForMember(src => src.AreaName, opt => opt.MapFrom(des => des.Area.AreaName));
            mc.CreateMap<ShopViewModel, TblShop>();

            mc.CreateMap<TblShop, ShopCreateViewModel>();
            mc.CreateMap<ShopCreateViewModel, TblShop>();

            mc.CreateMap<TblShop, ShopSearchViewModel>()
                .ForMember(src => src.AreaName, opt => opt.MapFrom(des => des.Area.AreaName))
                .ForMember(src => src.Username, opt => opt.MapFrom(des => des.Account.Username));
            mc.CreateMap<ShopViewModel, TblShop>();
        }
    }
}
