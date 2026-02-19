using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessTimeGym.Data.PaymentType.Queries
{
    public interface IPaymentTypeQueries
    {
        List<SelectListItem> ListofPaymentTypes();
    }
}