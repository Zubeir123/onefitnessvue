using FitnessTimeGym.Model.UserMaster;
using FitnessTimeGym.ViewModel.Messages;

namespace FitnessTimeGym.Services.Messages
{
    public interface IEmailSender
    {
        string SendMailusingSmtp(MessageTemplate messageTemplate);
        string CreateVerificationEmail(UserMasterModel user, string token);
        string CreateRegistrationVerificationEmail(UserMasterModel user, string token);
    }
}