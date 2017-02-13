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
    public class PendingPaymentScheduleController : BaseController
    {
        // GET: PendingPaymentSchedule
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PendingPaymentSchedule(string Client = "")
        {
            var list = new ReportRepository().GetPendingPaymentScheduleReport(Client);

            Session["PendingPaymentSchedule"] = list;

            return PartialView("_PendingPaymentSchedule", list);
        }
    }
}