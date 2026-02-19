using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.MenuMaster;



namespace FitnessTimeGym.Data.MenuMaster.Queries
{
    public interface IMenuMasterQueries
    {
    
        List<MenuMasterModel> GetMenuByRoleId(int? roleId, int? menuCategoryId);
    }
}