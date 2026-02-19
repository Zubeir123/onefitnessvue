using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.MemberRegistration;
using FitnessTimeGym.ViewModel.MemberRegistration;

namespace FitnessTimeGym.Data.MemberRegistration.Queries
{
    public interface IMemberRegistrationQueries
    {
        List<SelectListItem> ListofGender();
        bool CheckMemberMobileNoExists(string mobileNo);
        bool CheckMemberEmailExists(string emailId);
        IQueryable<MemberRegistrationGrid> ShowAllMemberRegistration(string sortColumn, string sortColumnDir,
            string search);

        EditMemberRegistrationViewModel GetMemberRegistrationById(long? memberId);
        MemberRegistrationProfileViewModel GetMemberRegistrationProfileById(long? memberId);
        List<MemberRenewalRespone> GetMemberList(string membername);
        MemberRegistrationModel GetMemberDetailsByMemberId(long? memberId);
    }
}