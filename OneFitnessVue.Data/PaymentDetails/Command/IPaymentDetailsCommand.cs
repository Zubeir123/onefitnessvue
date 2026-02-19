using FitnessTimeGym.Model.PaymentDetails;

namespace FitnessTimeGym.Data.PaymentDetails.Command
{
    public interface IPaymentDetailsCommand
    {
        string Add(PaymentDetailsModel paymentDetailsModel);
    }
}