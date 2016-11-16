using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class InvoiceController : BaseController
    {
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Invoice()
        {
            return View();
        }
        public ActionResult PreviousInvoice()
        {
            return View();
        }
        public ActionResult PendingProjects()
        {
            return View();
        }

    }
}