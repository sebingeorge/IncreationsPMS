using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class EnquiryBookingController : BaseController
    {
        // GET: EnquiryBooking
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EnquiryBooking()
        {
            return View();
        }
        public ActionResult PendingEnquiryStatus()
        {
            return View();
        }
        public ActionResult EnquiryStatus()
        {
            return View();
        }
        public ActionResult Projects()
        {
            return View();
        }
        public ActionResult ProjectStatus()
        {
            return View();
        }

    }
}