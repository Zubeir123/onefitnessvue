using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FitnessTimeGym.Common;
using FitnessTimeGym.Common.Notification;

using FitnessTimeGym.Data.GeneralSetting.Queries;
using FitnessTimeGym.Data.Reporting.Command;
using FitnessTimeGym.Data.Reporting.Queries;
using FitnessTimeGym.Model.Reporting;
using FitnessTimeGym.ViewModel.MemberRegistration;
using FitnessTimeGym.Web.Filters;

namespace FitnessTimeGym.Web.Areas.Service.Controllers
{
    [Area("Service")]
    [AuthorizeUser]
    public class ReceiptController : Controller
    {
        private readonly IGeneralSettingQueries _generalSettingQueries;
        private readonly IReportingQueries _reportingQueries;
        private readonly IReportingCommand _reportingCommand;

        private readonly INotificationService _notificationService;

        public ReceiptController(IGeneralSettingQueries generalSettingQueries,
            IReportingQueries reportingQueries,
            IReportingCommand reportingCommand,
           
            INotificationService notificationService
          )
        {
            _generalSettingQueries = generalSettingQueries;
            _reportingQueries = reportingQueries;
            _reportingCommand = reportingCommand;
          
            _notificationService = notificationService;
          
        }

        [HttpGet]
        public IActionResult Generate(string memberId, string paymentId)
        {

            if (!Regex.IsMatch(paymentId, "^[0-9]*$"))
            {
                _notificationService.DangerNotification("Message", "Some Thing Went Wrong !");
                return RedirectToAction("Dashboard", "Dashboard");
            }

            if (!Regex.IsMatch(memberId, "^[0-9]*$"))
            {
                _notificationService.DangerNotification("Message", "Some Thing Went Wrong !");
                return RedirectToAction("Dashboard", "Dashboard");
            }


            var recepitDetails = _generalSettingQueries.GetRecepitDetails(memberId, paymentId);
            var userid = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var receiptViewModel = new ReceiptViewModel()
            {
                GeneralSettings = _generalSettingQueries.GetActiveGeneralSettings(),
                RecepitDetails = recepitDetails,
                InvoiceNo = recepitDetails.InvoiceNo,
                InvoiceDate = DateTime.Now.ToString("dd/MM/yyyy")
            };

            var receiptHistoryModel = new ReceiptHistoryModel()
            {
                InvoiceNo = recepitDetails.InvoiceNo,
                CreatedOn = DateTime.Now,
                MemberNo = recepitDetails.MemberNo,
                CreatedBy = userid,
                ReceiptHistoryId = 0
            };
            _reportingCommand.SaveReceiptHistory(receiptHistoryModel);

            return View(receiptViewModel);
        }
    }
}
