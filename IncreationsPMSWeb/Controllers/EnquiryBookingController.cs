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
            EnquiryBooking.EnquiryDate = DateTime.Now;
            return View(EnquiryBooking);
            
        }
        [HttpPost]
        public ActionResult EnquiryBooking(EnquiryBooking model)
        {
           if (!ModelState.IsValid)
            {
                FillDropdowns();
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;
            model.OrganizationId = OrganizationId;
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
            FillEnquiryStatus();
            FillProjectType();
            ViewBag.Fromdate = FYStartdate;
            //var repo = new EnquiryBookingRepository();
            //IEnumerable<PendingEnquiryStatus> PendingEnquiryStatus = repo.GetPendingEnquiryStatus();
            return View();
        }

        public ActionResult PendingEnquiryStatusPartial(int? ProjectTypeId, int? EnquiryStatusId, DateTime? FromDate, DateTime? ToDate, string EnquiryClient = "")
        {

            FromDate = FromDate ?? FYStartdate;
            ToDate = ToDate ?? DateTime.Now;
            var repo = new EnquiryBookingRepository();
            IEnumerable<PendingEnquiryStatus> PendingEnquiryStatus = repo.GetPendingEnquiryStatus(ProjectTypeId, EnquiryStatusId, FromDate, ToDate, EnquiryClient);
            return PartialView("_PendingEnquiryStatus", PendingEnquiryStatus);
        }


        public ActionResult EnquiryStatus(int? EnquiryId)
        {
            FillEnquiryStatus();
            EnquiryBookingRepository repo = new EnquiryBookingRepository();
            EnquiryStatus model = repo.GetEnquiryStatusDetails(EnquiryId ?? 0);
           return View(model);
        }



        [HttpPost]
        public ActionResult EnquiryStatus(EnquiryStatus model)
        {
            if (!ModelState.IsValid)
            {
                FillEnquiryStatus();
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }

            var repo = new EnquiryBookingRepository();
         
            {
                var result = new EnquiryBookingRepository().UpdateEnquiryStatus(model);
                if (result.EnquiryId > 0)
                {
                    TempData["Success"] = "Updated Successfully!";
                    ////TempData["EnquiryRef"] = result.EnquiryRef;
                    return RedirectToAction("PendingEnquiryStatus");
                    //return RedirectToAction("Projects", "EnquiryBooking");
                }

                else
                {
                    return RedirectToAction("Edit");
                }

            }
            

        }
        void FillEnquiryStatus()
        {

            ViewBag.StatusType = new SelectList((new DropdownRepository()).GetStatusType(), "Id", "Name");

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
            FillDropdownsProject();
            Projects Projects = new Projects();
            Projects.ProjectTask = new List<ProjectTask>();
            Projects.ProjectTask.Add(new ProjectTask());
            Projects.ProjectPaymentSchedule = new List<ProjectPaymentSchedule>();
            Projects.ProjectPaymentSchedule.Add(new ProjectPaymentSchedule());
            Projects.ProjectRefNo = new ProjectsRepository().GetRefNo(Projects);
            Projects.ProjectDate = DateTime.Now;
            Projects.ProjectTask[0].StartDate= DateTime.Now;
            Projects.ProjectTask[0].EndDate = DateTime.Now;
            Projects.ProjectPaymentSchedule[0].Date = DateTime.Now;
            //Projects.ProjectPaymentSchedule[0].Date = DateTime.Now;
            return View(Projects);
        }
        void FillDropdownsProject()
        {
            FillClient();
        }
        void FillClient()
        {

            ViewBag.ClientId = new SelectList((new DropdownRepository()).GetClient(), "Id", "Name");

        }
        public JsonResult GetClientContactDetailsByKey(int Id)
        {
            var details = (new ProjectsRepository()).GetClientContactDetails(Id);
            return Json(new { Success = true, MobileNo = details.MobileNo, Email = details.Email, Address1 = details.Address1, Address2 = details.Address2, Address3 = details.Address3 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Projects(Projects model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdownsProject();
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;
            var repo = new ProjectsRepository();
            //bool isexists = repo.IsFieldExists(repo.ConnectionString(), "Projects", "SubName", model.SubName, null, null);
            //if (!isexists)
            {
                var result = new ProjectsRepository().Insert(model);
                if (result.ProjectId > 0)
                {

                    TempData["Success"] = "Added Successfully!";
                    TempData["ProjectRefNo"] = result.ProjectRefNo;
                    return RedirectToAction("Projects");
                }

                else
                {
                    TempData["error"] = "Oops!!..Something Went Wrong!!";
                    TempData["ProjectRefNo"] = null;
                    return View("Projects", model);
                }

            }
            //else
            //{

            //    TempData["error"] = "This Name Alredy Exists!!";
            //    TempData["ProjectRefNo"] = null;
            //    return View("Create", model);
            //}

        }

        public ActionResult PendingProjects()
        {
            ViewBag.Fromdate = FYStartdate;
            return View();
            //var repo = new ProjectsRepository();
            //IEnumerable<PendingProjects> PendingProjects = repo.GetPendingProjects();
            //return View(PendingProjects);
        }


        public ActionResult PendingProjectsPartial(DateTime? FromDate, DateTime? ToDate, string ClientName = "")
        {

            FromDate = FromDate ?? FYStartdate;
            ToDate = ToDate ?? DateTime.Now;


            var repo = new ProjectsRepository();
            IEnumerable<PendingProjects> PendingProjects = repo.GetPendingProjects(FromDate, ToDate, ClientName);
            return PartialView("_PendingProjects", PendingProjects);
        }


        public ActionResult ProjectsDisplay(int ProjectId)
            //(int? ProjectId)
        {
            if (ProjectId == 0)
            {
                TempData["error"] = "That was an invalid/unknown request.";
                return RedirectToAction("Index", "Home");
            }
            ProjectsRepository repo = new ProjectsRepository();
            FillDropdownsProject();
            Projects model = repo.GetProjectsDetails(ProjectId);
            //(ProjectId ?? 0)
            //model.ProjectTask = new List<ProjectTask>();
            //model.ProjectTask.Add(new ProjectTask());
            //model.ProjectPaymentSchedule = new List<ProjectPaymentSchedule>();
            //model.ProjectPaymentSchedule.Add(new ProjectPaymentSchedule());
            model.ProjectTask = repo.GetTaskDetails(ProjectId);

            if (model.ProjectTask.Count == 0)
                model.ProjectTask.Add(new ProjectTask());

            model.ProjectPaymentSchedule = repo.GetPaymentDetails(ProjectId);

            if (model.ProjectPaymentSchedule.Count == 0)
                model.ProjectPaymentSchedule.Add(new ProjectPaymentSchedule());

            return View("Projects", model);
        }
        [HttpPost]
        public ActionResult ProjectsDisplay(Projects model)
        {
            //ViewBag.Title = "Edit";
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;

            FillDropdownsProject();

            var repo = new ProjectsRepository();
            //var result1 = new ProjectsRepository().CHECK(model.ProjectId);
            //if (result1 > 0)
            //{
            //    TempData["error"] = "Sorry! Already Used.";
            //    TempData["GRNNo"] = null;
            //    return View("Edit", model);
            //}

            //else
            {
                try
                {
                    var result = new ProjectsRepository().UpdateProjects(model);
                    if (result.ProjectId > 0)
                    {
                        TempData["success"] = "Updated successfully. (" + result.ProjectRefNo + ")";
                        TempData["ProjectRefNo"] = result.ProjectRefNo;
                       return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                //catch (SqlException)
                //{
                //    TempData["error"] = "Some error occured while connecting to database. Please check your network connection and try again.";
                //}
                //catch (NullReferenceException)
                //{
                //    TempData["error"] = "Some required data was missing. Please try again.";
                //}
                catch (Exception)
                {
                    TempData["error"] = "Some error occured. Please try again.";
                }
                return RedirectToAction("Index", "Home");
            }

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