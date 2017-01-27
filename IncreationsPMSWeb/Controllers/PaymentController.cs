using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDAL;
using IncreationsPMSDomain;

namespace IncreationsPMSWeb.Controllers
{
    public class PaymentController : BaseController
    {
        // GET: Payment

        public ActionResult Payment( int ProjectWorkDetailsId)
        {
            PaymentModeDropDown();
            Payment objPayment = new PaymentRepository().GetNewPayment(ProjectWorkDetailsId);
            var internalid = PaymentRepository.GetNextRefNo();
            objPayment.PaymentRefNo = internalid;
            objPayment.PaymentDate = DateTime.Now;
            return View("Payment", objPayment);
        }
      
        public void PaymentModeDropDown()
        {
            ViewBag.PaymentModeList = new SelectList(new DropdownRepository().FillPaymentMode(), "Id", "Name");
        }
        [HttpPost]
        public ActionResult Payment(Payment model)
        {
            //model.TranDate = System.DateTime.Now;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new PaymentRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");

        }

        public ActionResult PreviousPayment()
        {
            return View();
        }
        
        public ActionResult PreviousPaymentPartial(string SubName = "")
        {
            List<Payment> obj = new List<Payment>();
            obj = new PaymentRepository().PaymentList(SubName);

            return PartialView("_PreviousPaymentList", obj);
        }

              
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SubContractorPayment(string ProjectEnquiry = "", string  TaskName="", string SubName="")
        {

            return PartialView("_SubContractorPayment", new PaymentRepository().PendingSubcontractorPayment(ProjectEnquiry, TaskName, SubName));
        }
      
      

        public ActionResult Update(int id)
        {
            if (id == 0)
            {
                TempData["error"] = "That was an invalid/unknown request.";
                return RedirectToAction("Index", "Home");
            }
            PaymentRepository repo = new PaymentRepository();
            PaymentModeDropDown();

            Payment model = repo.PaymentView(id);
            return View("Payment", model);
        }
        [HttpPost]
        public ActionResult Update(Payment model)
        {
            //ViewBag.Title = "Update";

            {

                try
                {
                   var ref_no = new PaymentRepository().UpdatePayment(model);
                    TempData["success"] = "Updated Successfully (" + ref_no + ")";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Oops! Something went wrong!";
                    TempData["PaymentRefNo"] = null;
                    return RedirectToAction("Edit", new { id = model.PaymentId });
                }
            }
        }
        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";

            {

                try
                {
                    var ref_no = new PaymentRepository().DeletePayment(Id);
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
        public ActionResult PayableReport()
        {
            return View();
        }
        public ActionResult paymentFollowup()
        {
            return View();
        }
    }
}