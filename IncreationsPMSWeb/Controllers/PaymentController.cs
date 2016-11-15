using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class PaymentController : BaseController
    {
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult PreviousPayment()
        {
            return View();
        }
        public ActionResult PayableReport()
        {
            return View();
        }
    }
}