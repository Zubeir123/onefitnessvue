using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FitnessTimeGym.Common;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.MenuCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace FitnessTimeGym.Data.MenuCategory.Queries
{
    public class MenuCategoryQueries : IMenuCategoryQueries
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        private readonly IMemoryCache _cache;
        public MenuCategoryQueries(FitnessTimeGymContext FitnessTimeGymContext, IMemoryCache cache)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
            _cache = cache;
        }

        public List<MenuCategoryModel> GetCategoryByRoleId(int? roleId)
        {

            var key = $"{AllMemoryCacheKeys.MenuCategoryKey}_{roleId}";
            List<MenuCategoryModel> menuCategory;
            if (_cache.Get(key) == null)
            {
                var result = (from category in _FitnessTimeGymContext.MenuCategorys.AsNoTracking()
                              orderby category.SortingOrder ascending
                              where category.RoleId == roleId
                              select category).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7),
                    Priority = CacheItemPriority.Normal
                };

                menuCategory = _cache.Set<List<MenuCategoryModel>>(key, result, cacheExpirationOptions);
            }
            else
            {
                menuCategory = _cache.Get(key) as List<MenuCategoryModel>;
            }

            return menuCategory;
        }

     
    }
}