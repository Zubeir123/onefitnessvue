using System.Collections.Generic;
using FitnessTimeGym.ViewModel.Documents;
using FitnessTimeGym.ViewModel.PaymentDetails;

namespace FitnessTimeGym.ViewModel.MemberRegistration
{
    public class MemberMainViewModel
    {
        public MemberRegistrationProfileViewModel MemberDetails { get; set; }
        public List<PaymentDetailsViewModel> ListofPayments { get; set; }
        public List<AttachmentGridViewModel> ListofDocuments { get; set; }
    }
}