using System.Collections.Generic;
using FitnessTimeGym.ViewModel.Reporting;

namespace FitnessTimeGym.Data.Reporting.Queries
{
    public interface IReportingQueries
    {
        List<RenewalReportViewModel> Get_RenewalReport(RenewalReportingViewModel renewalReport);
        List<YearwiseChartResponseViewModel> YearwiseChartRenewedResponse();
        List<YearwiseChartResponseViewModel> YearwiseChartNewResponse();
        int? GetRenewedMonthlyCount();
        int? GetNewRegistrationMonthlyCount();
        List<MembershipTypeViewModel> GetTopMembershipTypeInYear();
        int? GetNewRegistrationDailyCount();
        int? GetRefundMonthlyCount();
    }
}