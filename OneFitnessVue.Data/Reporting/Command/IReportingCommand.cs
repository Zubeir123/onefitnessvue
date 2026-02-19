using FitnessTimeGym.Model.Reporting;

namespace FitnessTimeGym.Data.Reporting.Command
{
    public interface IReportingCommand
    {
        void SaveReceiptHistory(ReceiptHistoryModel receiptHistoryModel);
    }
}