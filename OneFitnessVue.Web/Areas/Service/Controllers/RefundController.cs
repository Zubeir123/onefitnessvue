using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FitnessTimeGym.Common;
using FitnessTimeGym.Common.Notification;
using FitnessTimeGym.Data.MemberRegistration.Command;
using FitnessTimeGym.Data.PaymentDetails.Command;
using FitnessTimeGym.Data.PaymentDetails.Queries;
using FitnessTimeGym.Model.PaymentDetails;
using FitnessTimeGym.ViewModel.PaymentDetails;
using FitnessTimeGym.Web.Filters;

namespace FitnessTimeGym.Web.Areas.Service.Controllers
{
    [Area("Service")]
    [AuthorizeUser]
    public class RefundController : Controller
    {
        private readonly IPaymentDetailsQueries _paymentDetailsQueries;
      
        public RefundController(IPaymentDetailsQueries paymentDetailsQueries
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
