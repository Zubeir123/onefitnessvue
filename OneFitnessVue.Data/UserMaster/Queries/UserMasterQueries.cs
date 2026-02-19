using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using FitnessTimeGym.Data.EFContext;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.UserMaster;
using FitnessTimeGym.ViewModel.Usermaster;

namespace FitnessTimeGym.Data.UserMaster.Queries
{
    public class UserMasterQueries : IUserMasterQueries
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        public UserMasterQueries(FitnessTimeGymContext FitnessTimeGymContext)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
        }

        public UserMasterModel GetUserById(int? userId)
        {
            try
            {
                return _FitnessTimeGymContext.UserMasters.Find(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckUsernameExists(string username)
        {
            try
            {
                var result = (from menu in _FitnessTimeGymContext.UserMasters
                    where menu.UserName == username
                    select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserMasterModel GetUserByUsername(string username)
        {
            try
            {
                var result = (from usermaster in _FitnessTimeGymContext.UserMasters
                    where usermaster.UserName == username
                    select usermaster).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username)
        {
            var userdata = (from tempuser in _FitnessTimeGymContext.UserMasters
                
                
                where tempuser.UserName == username
                select new CommonUserDetailsViewModel()
                {
                    FirstName = tempuser.FirstName,
                    EmailId = tempuser.EmailId,
                    LastName = tempuser.LastName,
                    RoleId = 2,
                    UserId = tempuser.UserId,
                    RoleName = "SystemUser",
                    Status = tempuser.Status,
                    UserName = tempuser.UserName,
                    PasswordHash = tempuser.PasswordHash,
                    MobileNo = tempuser.MobileNo
                }).FirstOrDefault();

            return userdata;
        }
    }
}