using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FitnessTimeGym.Common.Notification;
using FitnessTimeGym.Data.Installments.Queries;
using FitnessTimeGym.Data.MemberRegistration.Command;
using FitnessTimeGym.Data.MemberRegistration.Queries;
using FitnessTimeGym.Data.MembershipType.Queries;
using FitnessTimeGym.Data.PaymentDetails.Command;
using FitnessTimeGym.Data.PaymentDetails.Queries;
using FitnessTimeGym.Data.PaymentType.Queries;
using FitnessTimeGym.Data.TaxMaster.Queries;
using FitnessTimeGym.Data.WorkOut.Queries;
using FitnessTimeGym.Web.Filters;
using System;
using System.Linq;

namespace FitnessTimeGym.Web.Areas.Service.Controllers
{
    [Area("Service")]
    [AuthorizeUser]
    public class RenewalController : Controller
    {
   
        private readonly IPaymentDetailsQueries _paymentDetailsQueries;

        public RenewalController(
           
            IPaymentDetailsQueries paymentDetailsQueries
   
          )
        {

            _paymentDetailsQueries = paymentDetailsQueries;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

      

    }
}
