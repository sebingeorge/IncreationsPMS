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
    public class OverDueTaskReportController : BaseController
    {
        // GET: OverDueTaskReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OverDueTaskSummary(string Client = "")
        {
            var list = new ReportRepository().GetOverDueTaskReport(Client);

            Session["OverDueTaskSummary"] = list;

            return PartialView("_OverDueTaskSummary", list);
        }
    }
}