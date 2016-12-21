using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDomain;
using IncreationsPMSDAL;

namespace IncreationsPMSWeb.Controllers
{
    public class SizeController :  BaseController
    {
        // GET: Size
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Size model)
        {
            //model.OrganizationId = OrganizationId;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID.ToString();

            var repo = new SizeRepository();
            //bool isexists = repo.IsFieldExists(repo.ConnectionString(), "Size", "SizeName", model.SizeName, null, null);
            //if (!isexists)
            //{
                var result = new SizeRepository().InsertSize(model);
                if (result.SizeCode > 0)
                {

                    TempData["Success"] = "Added Successfully!";
                    TempData["SizeUserCode"] = result.SizeUserCode;
                    return RedirectToAction("Create");
                }

                else
                {
                    TempData["error"] = "Oops!!..Something Went Wrong!!";
                    TempData["SizeUserCode"] = null;
                    return View("Create", model);
                }

            }
            //else
            //{

            //    TempData["error"] = "This Name Alredy Exists!!";
            //    TempData["SizeUserCode"] = null;
            //    return View("Create", model);
            //}

        //}
    }
}