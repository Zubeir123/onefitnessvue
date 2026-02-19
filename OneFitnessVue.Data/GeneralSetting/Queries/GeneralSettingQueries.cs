using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.GeneralSetting;
using FitnessTimeGym.ViewModel.MemberRegistration;
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace FitnessTimeGym.Data.GeneralSetting.Queries
{
    public class GeneralSettingQueries : IGeneralSettingQueries
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        private readonly IConfiguration _configuration;
        public GeneralSettingQueries(FitnessTimeGymContext FitnessTimeGymContext, IConfiguration configuration)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
            _configuration = configuration;
        }


        public GeneralSettingsModel GetActiveGeneralSettings()
        {
            var result = (from generalSetting in _FitnessTimeGymContext.GeneralSettings
                select generalSetting).FirstOrDefault();

            return result;
        }

        public RecepitDetailsViewModel GetRecepitDetails(string memberId, string paymentId)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            try
            {
                var param = new DynamicParameters();
                param.Add("@MemberID", memberId);
                param.Add("@PaymentID", paymentId);
                var result = connection.Query<RecepitDetailsViewModel>("Usp_GetRecepitDetails", param,null,true,0,CommandType.StoredProcedure).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}