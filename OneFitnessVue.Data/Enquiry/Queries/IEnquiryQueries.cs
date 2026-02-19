using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.Enquiry;
using FitnessTimeGym.ViewModel.Enquiry;

namespace FitnessTimeGym.Data.Enquiry.Queries
{
    public interface IEnquiryQueries
    {
        EnquiryModel GetEnquiryDetailsByEnquiryId(int? enquiryId);
        IQueryable<EnquiryGrid> ShowAllEnquiry(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetReasonsList();
        int? EnquiryCount();
    }
}