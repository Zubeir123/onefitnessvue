using FitnessTimeGym.Model.Enquiry;

namespace FitnessTimeGym.Data.Enquiry.Command
{
    public interface IEnquiryCommand
    {
        string Add(EnquiryModel enquiryModel);
        int Delete(EnquiryModel enquiryModel);
    }
}