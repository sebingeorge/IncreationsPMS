using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class ReportsController : BaseController
    {
        // GET: DailyActivityReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyActivityReport()
        {
            return View();
        }
        public ActionResult PreviousDailyActivityReport()
        {
            return View();
        }
        public ActionResult MaterialPlanningReport()
        {
            return View();
        }
    }
}