using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDAL;
using IncreationsPMSDomain;
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
            //ViewBag.Title = "Create";
            FillDropdowns();
            EnquiryBooking EnquiryBooking = new EnquiryBooking();
            EnquiryBooking.EnquiryRef = new EnquiryBookingRepository().GetRefNo(EnquiryBooking);
            return View(EnquiryBooking);

        }
        [HttpPost]
        public ActionResult EnquiryBooking(EnquiryBooking model)
        {
            //model.OrganizationId = OrganizationId;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID.ToString();

            var repo = new EnquiryBookingRepository();
            //bool isexists = repo.IsFieldExists(repo.ConnectionString(), "EnquiryBooking", "SubName", model.SubName, null, null);
            //if (!isexists)
            {
                var result = new EnquiryBookingRepository().Insert(model);
                if (result.EnquiryId > 0)
                {

                    TempData["Success"] = "Added Successfully!";
                    TempData["EnquiryRef"] = result.EnquiryRef;
                    return RedirectToAction("EnquiryBooking");
                }

                else
                {
                    TempData["error"] = "Oops!!..Something Went Wrong!!";
                    TempData["EnquiryRef"] = null;
                    return View("EnquiryBooking", model);
                }

            }
            //else
            //{

            //    TempData["error"] = "This Name Alredy Exists!!";
            //    TempData["SubRefNo"] = null;
            //    return View("Create", model);
            //}

        }
        void FillDropdowns()
        {
            FillClientType();
            FillProjectType();
            FillModeOfContact();
           
        }
        void FillClientType()
        {

            ViewBag.ClientType = new SelectList((new DropdownRepository()).GetClientType(), "Id", "Name");

        }
        void FillProjectType()
        {

            ViewBag.ProjectType = new SelectList((new DropdownRepository()).GetProjectType(), "Id", "Name");

        }
        void FillModeOfContact()
        {

            ViewBag.ModeOfContact = new SelectList((new DropdownRepository()).GetModeOfContact(), "Id", "Name");

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
        public ActionResult PendingProjects()
        {
            return View();
        }
        public ActionResult ProjectStatus()
        {
            return View();
        }
        public ActionResult PendingProjectStatus()
        {
            return View();
        }
        public ActionResult LabourEnteringReport()
        {
            return View();
        }

        public ActionResult LabourEntering()
        {
            return View();
        }
        public ActionResult MaterialEntering()
        {
            return View();
        }
        public ActionResult MaterialEnteringReport()
        {
            return View();
        }

        

    }
}