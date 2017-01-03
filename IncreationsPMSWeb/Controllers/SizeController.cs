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
        //public void FillCustomer()
        //{
        //    DropdownRepository repo = new DropdownRepository();
        //    var result = repo.SizeDropdown();
        //    ViewBag.CustomerList1 = new SelectList(result, "Id", "Name");

        //}
        //public ActionResult SizeList(int? page, string customer = "")
        //{
        //    int pageNumber = page ?? 1;
        //    return PartialView("_SizeListView", new SizeRepository().GetSizes(Size));
        //}
        public ActionResult PreviousList()
        {
            try
            {

                return View(new SizeRepository().GetSizes());
            }

            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message != null)
                    {
                        ErrorMessage = ErrorMessage + ex.InnerException.Message.ToString();
                    }
                }
                ViewData["Error"] = ErrorMessage;
                return View("ShowError");
            }
        }
        //public ActionResult Delete(int Id)
        //{
        //    ViewBag.Title = "Delete";
        //    Size objSize = new SizeRepository().GetSize(Id);
        //    return View("Create", objSize);

        //}

        //[HttpPost]
        //public ActionResult Delete(Size  model)
        //{
        //    int result = new SizeRepository().DeleteSize(model);

        //    if (result == 0)
        //    {
        //        TempData["Success"] = "Deleted Successfully!";
        //        TempData["SizeUserCode"] = model.SizeUserCode;
        //        return RedirectToAction("Create");
        //    }
        //    else
        //    {
        //        //if (result == 1)
        //        //{
        //        //    TempData["error"] = "Sorry!! You Cannot Delete This Designation It Is Already In Use";
        //        //    TempData["SizeUserCode"] = null;
        //        //}
        //        //else
        //        {
        //            TempData["error"] = "Oops!!..Something Went Wrong!!";
        //            TempData["SizeUserCode"] = null;
        //        }
        //        return RedirectToAction("Create");
        //    }

        //}
    }
}