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
        public ActionResult PreviousCustomer()
        {
            return View();
        }
        public ActionResult Item()
        {
            return View();
        }
        public ActionResult PreviousItem()
        {
            return View();
        }
        public ActionResult ItemGroup()
        {
            return View();
        }
        public ActionResult ItemSubGroup()
        {
            return View();
        }
        public ActionResult PreviousItemGroup()
        {
            return View();
        }
        public ActionResult SubContractor()
        {
            return View();
        }
        public ActionResult PreviousSubContractor()
        {
            return View();
        }
    }
}