using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.MenuCategory;


namespace FitnessTimeGym.Data.MenuCategory.Queries
{
    public interface IMenuCategoryQueries
    {
        List<MenuCategoryModel> GetCategoryByRoleId(int? roleId);
    }
}