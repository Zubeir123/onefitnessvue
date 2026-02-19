using System;
using Microsoft.EntityFrameworkCore;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.PaymentDetails;

namespace FitnessTimeGym.Data.PaymentDetails.Command
{
    public class PaymentDetailsCommand : IPaymentDetailsCommand
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        public PaymentDetailsCommand(FitnessTimeGymContext FitnessTimeGymContext)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
        }

        public string Add(PaymentDetailsModel paymentDetailsModel)
        {
            string result;

            using var transaction = _FitnessTimeGymContext.Database.BeginTransaction();
            _FitnessTimeGymContext.PaymentDetailsModels.Add(paymentDetailsModel);
            var resultpayment = _FitnessTimeGymContext.SaveChanges();

            if (resultpayment > 0)
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

    }

}