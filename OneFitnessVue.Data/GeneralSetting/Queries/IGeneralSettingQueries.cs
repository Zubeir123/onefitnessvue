using System.Linq;
using FitnessTimeGym.Model.GeneralSetting;
using FitnessTimeGym.ViewModel.MemberRegistration;

namespace FitnessTimeGym.Data.GeneralSetting.Queries
{
    public interface IGeneralSettingQueries
    {
        GeneralSettingsModel GetActiveGeneralSettings();
        RecepitDetailsViewModel GetRecepitDetails(string memberId, string paymentId);
    }
}