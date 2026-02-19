using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.UserMaster;
using FitnessTimeGym.ViewModel.Usermaster;

namespace FitnessTimeGym.Data.UserMaster.Queries
{
    public interface IUserMasterQueries
    {
        UserMasterModel GetUserById(int? userId);
        bool CheckUsernameExists(string username);
        UserMasterModel GetUserByUsername(string username);
        CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username);
       
    }
}