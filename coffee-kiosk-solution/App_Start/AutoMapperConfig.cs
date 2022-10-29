using AutoMapper;
using coffee_kiosk_solution.Data.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.App_Start
{
    public static class AutoMapperConfig
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.ConfigAccountModule();
                mc.ConfigCategoryModule();
                mc.ConfigProductModule();
                mc.ConfigCampaignModule();
                mc.ConfigAreaModule();
                mc.ConfigDiscountModule();
                mc.ConfigProductImageModule();
                mc.ConfigShopModule();
                mc.ConfigSupplyModule();
                mc.ConfigOrderModule();
                mc.ConfigOrderDetailModule();
                mc.ConfigRoleModule();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
