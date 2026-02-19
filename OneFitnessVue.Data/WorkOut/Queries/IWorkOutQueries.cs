using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitnessTimeGym.Data.WorkOut.Queries
{
    public interface IWorkOutQueries
    {
        List<SelectListItem> GetWorkOuts();
    }
}