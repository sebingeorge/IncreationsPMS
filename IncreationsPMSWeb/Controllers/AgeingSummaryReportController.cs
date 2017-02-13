using IncreationsPMSDAL;
using IncreationsPMSDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace IncreationsPMSWeb.Controllers
{
    public class AgeingSummaryReportController : BaseController
    {
        // GET: AgeingSummaryReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AgeingSummary(string Client = "")
        {
           var list = new ReportRepository().GetAgeingSummaryBasedCommittedDate(Client);

            Session["ageingdata"] = list;

            return PartialView("_AgeingSummary", list);
        }
    }
}