using AutoMapper;
using FitnessTimeGym.Model.Enquiry;
using FitnessTimeGym.Model.MemberRegistration;
using FitnessTimeGym.ViewModel.Enquiry;
using FitnessTimeGym.ViewModel.MemberRegistration;

namespace FitnessTimeGym.Web.Mapping
{
    public class FitnessTimeGymAutoMapper : Profile
    {
        public FitnessTimeGymAutoMapper()
        {
            CreateMap<MemberRegistrationViewModel, MemberRegistrationModel>();
            CreateMap<EditMemberRegistrationViewModel, MemberRegistrationModel>();
            CreateMap<EnquiryViewModel, EnquiryModel>();
        }
    }
}