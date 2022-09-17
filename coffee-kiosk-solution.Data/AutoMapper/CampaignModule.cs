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
    public static class CampaignModule
    {
        public static void ConfigCampaignModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblCampaign, CampaignViewModel>()
                .ForMember(src => src.AreaName, opt => opt.MapFrom(des => des.Area.AreaName));
            mc.CreateMap<CampaignViewModel, TblCampaign>();

            mc.CreateMap<TblCampaign, CampaignCreateViewModle>();
            mc.CreateMap<CampaignCreateViewModle, TblCampaign>();

            mc.CreateMap<TblCampaign, CampaignUpdateViewModel>();
            mc.CreateMap<CampaignUpdateViewModel, TblCampaign>();

            mc.CreateMap<TblCampaign, CampaignUpdateViewModel>();
            mc.CreateMap<CampaignUpdateViewModel, TblCampaign>();

            mc.CreateMap<TblCampaign, CampaignSearchViewModule>()
                .ForMember(src => src.AreaName, opt => opt.MapFrom(des => des.Area.AreaName));
            mc.CreateMap<CampaignSearchViewModule, TblCampaign>();
        }
    }
}
