using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using FitnessTimeGym.ViewModel.Installments;

namespace FitnessTimeGym.Data.Installments.Queries
{
    public interface IInstallmentQueries
    {
        List<SelectListItem> GetInstallments();
        InstallmentEditViewModel GetInstallmentByInstallmentId(int? installmentId);
    }
}