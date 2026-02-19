using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTimeGym.Data.EFContext;

namespace FitnessTimeGym.Data.PaymentType.Queries
{
    public class PaymentTypeQueries : IPaymentTypeQueries
    {
        private readonly FitnessTimeGymContext _FitnessTimeGymContext;
        public PaymentTypeQueries(FitnessTimeGymContext FitnessTimeGymContext)
        {
            _FitnessTimeGymContext = FitnessTimeGymContext;
        }

        public List<SelectListItem> ListofPaymentTypes()
        {
            var result =  new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Cash",
                    Value = "1"
                }
            };

            result.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select---"
            });

            return result;
        }

    }
}