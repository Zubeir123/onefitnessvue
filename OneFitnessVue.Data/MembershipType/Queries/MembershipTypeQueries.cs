using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.MembershipType;
using FitnessTimeGym.ViewModel.MembershipType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTimeGym.Data.MembershipType.Queries
{
    public class MembershipTypeQueries : IMembershipTypeQueries
    {
        private readonly FitnessTimeGymContext _context;

        public MembershipTypeQueries(FitnessTimeGymContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetAllMembershipTypes(RequestMembershipType requestMembershipType)
        {
            try
            {

                var membershiptypelist = _context.MembershipTypes
                    .Where(x => x.Status == true)
                    .Select(x => new SelectListItem()
                    {
                        Text = x.MembershipTypeName,
                        Value = x.MembershipTypeId.ToString()
                    }).ToList();

                membershiptypelist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return membershiptypelist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MembershipTypeModel MembershipTypeDetailsByMembershipTypeId(int? membershipTypeId)
        {
            var membershiptype = _context.MembershipTypes
                .FirstOrDefault(x => x.MembershipTypeId == membershipTypeId);

            return membershiptype;
        }
    }
}