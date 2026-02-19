using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.Enquiry;
using FitnessTimeGym.Model.MemberRegistration;

namespace FitnessTimeGym.Data.Enquiry.Command
{
    public class EnquiryCommand : IEnquiryCommand
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        private readonly IConfiguration _configuration;
        public EnquiryCommand(FitnessTimeGymContext FitnessTimeGymContext, IConfiguration configuration)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
            _configuration = configuration;
        }

        public string Add(EnquiryModel enquiryModel)
        {
            string result;

            using var transaction = _FitnessTimeGymContext.Database.BeginTransaction();
            _FitnessTimeGymContext.EnquiryModels.Add(enquiryModel);
            var resultmember = _FitnessTimeGymContext.SaveChanges();

            if (resultmember > 0)
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

        public int Delete(EnquiryModel enquiryModel)
        {
            _FitnessTimeGymContext.Entry(enquiryModel).State = EntityState.Deleted;
            return _FitnessTimeGymContext.SaveChanges();
        }

    }
}