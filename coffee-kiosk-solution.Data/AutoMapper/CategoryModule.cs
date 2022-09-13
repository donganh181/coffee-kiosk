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
    public static class CategoryModule
    {
        public static void ConfigCategoryModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<TblCategory, CategoryViewModel>();
            mc.CreateMap<CategoryViewModel, TblCategory>();

            mc.CreateMap<TblCategory, CategoryCreateViewModel>();
            mc.CreateMap<CategoryCreateViewModel, TblCategory>();

            mc.CreateMap<TblCategory, CategorySearchViewModel>();
            mc.CreateMap<CategorySearchViewModel, TblCategory>();
        }
    }
}
