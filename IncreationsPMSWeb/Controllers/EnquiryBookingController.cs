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
            //return View();
            var repo = new EnquiryBookingRepository();
            IEnumerable<PendingEnquiryStatus> PendingEnquiryStatus = repo.GetPendingEnquiryStatus();
            return View(PendingEnquiryStatus);
        }
    

        public ActionResult EnquiryStatus(int? EnquiryId)
        {
            EnquiryBookingRepository repo = new EnquiryBookingRepository();

            EnquiryStatus model = repo.GetEnquiryStatusDetails(EnquiryId ?? 0);

           
            //string internalId = "";
            //try
            //{
            //    internalId = DatabaseCommonRepository.GetNextDocNo(8);

            //}
            //catch (NullReferenceException nx)
            //{
            //    TempData["success"] = "";
            //    TempData["error"] = "Some required data was missing. Please try again.|" + nx.Message;
            //}
            //catch (Exception ex)
            //{
            //    TempData["success"] = "";
            //    TempData["error"] = "Some error occurred. Please try again.|" + ex.Message;
            //}

            //model.PurchaseRequestNo = internalId;
            //model.PurchaseRequestDate = System.DateTime.Today;
            //model.RequiredDate = System.DateTime.Today;
            //model.OrganizationId = OrganizationId;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID.ToString();

            return View(model);
        }



        [HttpPost]
        public ActionResult EnquiryStatus(EnquiryStatus model)
        {
           

            var repo = new EnquiryBookingRepository();
         
            {
                var result = new EnquiryBookingRepository().UpdateEnquiryStatus(model);
                if (result.EnquiryId > 0)
                {

                    return RedirectToAction("Projects", "EnquiryBooking");
                }

                else
                {
                    return RedirectToAction("Edit");
                }

            }
            

        }

        public ActionResult EnquiryCancel(int Id)
        {
                                
                   
            {
        
                var result = new EnquiryBookingRepository().UpdateEnquiryCancel(Id);
                                              
                    return RedirectToAction("index", "home");

            }

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