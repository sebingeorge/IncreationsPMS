using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class BOQController : BaseController
    {
        // GET: BOQ
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BOQPreparing()
        {
            return View();
        }
    }
}