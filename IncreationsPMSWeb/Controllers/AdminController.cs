using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncreationsPMSWeb.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Item()
        {
            return View();
        }
        public ActionResult ItemGroup()
        {
            return View();
        }
        public ActionResult SubContractor()
        {
            return View();
        }
    }
}