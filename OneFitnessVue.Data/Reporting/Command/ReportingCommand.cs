using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.Reporting;

namespace FitnessTimeGym.Data.Reporting.Command
{
    public class ReportingCommand : IReportingCommand
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        public ReportingCommand(FitnessTimeGymContext FitnessTimeGymContext)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
        }


        public void SaveReceiptHistory(ReceiptHistoryModel receiptHistoryModel)
        {
            _FitnessTimeGymContext.ReceiptHistory.Add(receiptHistoryModel);
            _FitnessTimeGymContext.SaveChangesAsync();
        }

    }
}