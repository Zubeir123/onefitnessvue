using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.MembershipType;
using FitnessTimeGym.ViewModel.MembershipType;

namespace FitnessTimeGym.Data.MembershipType.Queries
{
    public interface IMembershipTypeQueries
    {
        List<SelectListItem> GetAllMembershipTypes(RequestMembershipType requestMembershipType);
        MembershipTypeModel MembershipTypeDetailsByMembershipTypeId(int? membershipTypeId);
    }
}