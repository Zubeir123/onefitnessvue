using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.MemberRegistration;
using FitnessTimeGym.Model.PaymentDetails;

namespace FitnessTimeGym.Data.MemberRegistration.Command
{
    public class MemberRegistrationCommand : IMemberRegistrationCommand
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        private readonly IConfiguration _configuration;
        public MemberRegistrationCommand(FitnessTimeGymContext FitnessTimeGymContext, IConfiguration configuration)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
            _configuration = configuration;
        }

        public string Add(MemberRegistrationModel memberRegistration, PaymentDetailsModel paymentDetailsModel)
        {
            string result;

            using var transaction = _FitnessTimeGymContext.Database.BeginTransaction();
            _FitnessTimeGymContext.MemberRegistrationModels.Add(memberRegistration);
            var resultmember = _FitnessTimeGymContext.SaveChanges();

            paymentDetailsModel.MemberID = memberRegistration.MemberId;
            _FitnessTimeGymContext.PaymentDetailsModels.Add(paymentDetailsModel);
            var resultpayment = _FitnessTimeGymContext.SaveChanges();

            if (resultmember > 0 && resultpayment > 0)
            {
                transaction.Commit();
                result = "success";
            }
            else
            {
                transaction.Rollback();
                result = "failed";
            }

            return result;
        }

      

        public string Update(MemberRegistrationModel memberRegistration, PaymentDetailsModel paymentDetailsModel)
        {
            string result;

            using var transaction = _FitnessTimeGymContext.Database.BeginTransaction();
            _FitnessTimeGymContext.Entry(memberRegistration).State = EntityState.Modified;
            var resultmember = _FitnessTimeGymContext.SaveChanges();

            _FitnessTimeGymContext.Entry(paymentDetailsModel).State = EntityState.Modified;
            var resultpayment = _FitnessTimeGymContext.SaveChanges();


            if (resultmember > 0 && resultpayment > 0)
            {
                transaction.Commit();
                result = "success";
            }
            else
            {
                transaction.Rollback();
                result = "failed";
            }

            return result;
        }

        public long GetInvoiceNo()
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            try
            {
                long invoiceId = 0;
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@InvoiceId", dbType: DbType.Int64, direction: ParameterDirection.Output, size: 50);
                var result = connection.Execute("Usp_GetNewInvoiceId", param, transaction, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    transaction.Commit();
                    invoiceId = param.Get<long>("@InvoiceId");
                }
                else
                {
                    transaction.Rollback();
                }

                return invoiceId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeactivateMember(MemberRegistrationModel memberRegistration)
        {
            _FitnessTimeGymContext.Entry(memberRegistration).State = EntityState.Modified;
            return _FitnessTimeGymContext.SaveChanges();
        }

    }
}