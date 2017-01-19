using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDAL;
using IncreationsPMSDomain;


namespace IncreationsPMSWeb.Controllers
{
    public class BOQController : BaseController
    {
        // GET: BOQ
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BOQPreparing(int Id)
        {
            SubContractorDropDown();
            BOQPreparing objBOQPreparing = new BOQPreparingRepository().GetNewBOQPreparing(Id);
            //objBOQPreparing.projectWorkRefNo = new BOQPreparingRepository().GetRefNo(objBOQPreparing);
            var internalid = BOQPreparingRepository.GetNextRefNo();
            objBOQPreparing.projectWorkRefNo = internalid;

            objBOQPreparing.projectWorkDate = DateTime.Now;
            objBOQPreparing.BOQPreparingItem = new BOQPreparingRepository().GetTaskDetails(Id);
            foreach (var item in objBOQPreparing.BOQPreparingItem)
            {
                item.BOQPreparingItemWork = new List<BOQPreparingItemWork>();
                item.BOQPreparingItemWork.Add(new BOQPreparingItemWork());
                item.BOQPreparingItemWork[0].PlanedStartDate = DateTime.Now;
                item.BOQPreparingItemWork[0].PlanedEndDate = DateTime.Now;
            }


            return View("BOQPreparing", objBOQPreparing);
        }
        [HttpPost]
        public ActionResult BOQPreparing(BOQPreparing model)
        {
            //model.TranDate = System.DateTime.Now;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new BOQPreparingRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");

        }
        public void SubContractorDropDown()
        {
            ViewBag.SubContractorList = new SelectList(new DropdownRepository().FillSubContractor(), "Id", "Name");
        }
        public ActionResult PendingBOQPreparing(string ClientName = "")
        {

            return PartialView("_PendingBOQPreparing", new BOQPreparingRepository().GetNewBOQPreparing(ClientName));
        }
        public ActionResult ListIndex()
        {
            return View();
        }
        public ActionResult BOQPrepareList()
        {
            List<BOQPreparing> obj = new List<BOQPreparing>();
            obj = new BOQPreparingRepository().GetBOQPreparingList();

            return PartialView("_BOQListGrid", obj);
        }
        public ActionResult ShowDetails(int id)
        {
            if (id == 0)
            {
                TempData["error"] = "That was an invalid/unknown request.";
                return RedirectToAction("Index", "Home");
            }
            BOQPreparingRepository repo = new BOQPreparingRepository();
            SubContractorDropDown();
            BOQPreparing model = repo.GetBOQPreparingView(id);
            return View("BOQPreparing", model);
        }
        [HttpPost]
        public ActionResult ShowDetails(BOQPreparing model)
        {
            //ViewBag.Title = "Edit";
            //model.OrganizationId = OrganizationId;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID.ToString();

            var repo = new BOQPreparingRepository();

            //var result1 = new BOQPreparingRepository().CHECK(model.QuerySheetId);
            //if (result1 > 0)
            //{
            //    TempData["error"] = "Sorry! Already Used!";
            //    TempData["QuerySheetRefNo"] = null;
            //    return View("Edit", model);
            //}

            //else
            //{
                try
                {

                    int row = new BOQPreparingRepository().UpdateBOQPreparing(model);

                    TempData["success"] = "Updated successfully" ;
                    TempData["error"] = "";
                    return RedirectToAction("Index");
                }
                //catch (SqlException)
                //{
                //    TempData["error"] = "Some error occured while connecting to database. Please check your network connection and try again.";
                //}
                catch (NullReferenceException)
                {
                    TempData["error"] = "Some required data was missing. Please try again.";
                }
                catch (Exception)
                {
                    TempData["error"] = "Some error occured. Please try again.";
                }
                return RedirectToAction("Index");
            //}

        }
        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";

            {

                try
                {
                    var ref_no = new BOQPreparingRepository().DeleteBOQPreparing(Id);
                    TempData["success"] = "Deleted Successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Oops! Something went wrong!";
                    //TempData["SubRefNo"] = null;
                    return RedirectToAction("ShowDetails", new { id = Id });
                }
            }
        }

        public ActionResult WorkStatus()
        {
            return View();
        }
    }
}