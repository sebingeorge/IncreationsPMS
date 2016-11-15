using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class ReceiptController : BaseController
    {
        // GET: Collecting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Receipt()
        {
            return View();
        }
        public ActionResult PreviousReceipt()
        {
            return View();
        }
        public ActionResult ReceivableReport()
        {
            return View();
        }
    }
}