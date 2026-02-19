using System.Collections.Generic;
using System.Linq;
using FitnessTimeGym.Model.PaymentDetails;
using FitnessTimeGym.ViewModel.MemberRegistration;
using FitnessTimeGym.ViewModel.PaymentDetails;

namespace FitnessTimeGym.Data.PaymentDetails.Queries
{
    public interface IPaymentDetailsQueries
    {
        List<PaymentDetailsViewModel> ListofPayments(long? memberId);

        IQueryable<PaymentDetailsViewModel> ShowAllPaymentsHistory(string sortColumn, string sortColumnDir,
            string search);


    }
}