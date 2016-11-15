using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class PurchaseController : BaseController
    {
        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PurchaseIndent()
        {
            return View();
        }
        public ActionResult PreviousPurchaseIndent()
        {
            return View();
        }
        public ActionResult PendingPurchaseIndent()
        {
            return View();
        }
        public ActionResult PurchaseOrder()
        {
            return View();
        }
        public ActionResult PreviousPurchaseOrder()
        {
            return View();
        }
        public ActionResult PendingPurchaseOrder()
        {
            return View();
        }
        public ActionResult GRN()
        {
            return View();
        }
        public ActionResult PreviousGRN()
        {
            return View();
        }
    }
}