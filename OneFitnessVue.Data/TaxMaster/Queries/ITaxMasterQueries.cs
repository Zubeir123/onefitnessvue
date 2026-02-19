using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Model.TaxMaster;

namespace FitnessTimeGym.Data.TaxMaster.Queries
{
    public interface ITaxMasterQueries
    {
        List<SelectListItem> GetTaxList();
        TaxMasterModel GetTaxDetailsbyTaxId(int taxId);
    }
}