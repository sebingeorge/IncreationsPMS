using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDAL;
using IncreationsPMSDomain;

namespace IncreationsPMSWeb.Controllers
{
    public class SubContractorController : BaseController
    {
        // GET: SubContractor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            SubContractor SubContractor = new SubContractor();
            SubContractor.SubRefNo = new SubContractorRepository().GetRefNo(SubContractor);
            return View(SubContractor);
        }
        [HttpPost]
               public ActionResult Create(SubContractor model)
        {
            //model.OrganizationId = OrganizationId;
            //model.CreatedDate = System.DateTime.Now;
            //model.CreatedBy = UserID.ToString();

            var repo = new SubContractorRepository();
            bool isexists = repo.IsFieldExists(repo.ConnectionString(), "SubContractor", "SubName", model.SubName, null, null);
            if (!isexists)
            {
                var result = new SubContractorRepository().Insert(model);
                if (result.SubContractorId > 0)
                {

                    TempData["Success"] = "Added Successfully!";
                    TempData["SubRefNo"] = result.SubRefNo;
                    return RedirectToAction("Create");
                }

                else
                {
                    TempData["error"] = "Oops!!..Something Went Wrong!!";
                    TempData["SubRefNo"] = null;
                    return View("Create", model);
                }

            }
            else
            {

                TempData["error"] = "This Name Alredy Exists!!";
                TempData["SubRefNo"] = null;
                return View("Create", model);
            }

        }
        public ActionResult SubContratorList()
        {
            List<SubContractor> obj = new List<SubContractor>();
            obj = new SubContractorRepository().GetSubContractor();

            return PartialView("_Grid", obj);
        }

        public ActionResult Edit(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    SubContractor SubContractor = new SubContractor();
                    SubContractor = new SubContractorRepository().GetSubContractorView(id);

                    return View("Create", SubContractor);
                }
                else
                {
                    TempData["error"] = "That was an invalid/unknown request. Please try again.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (InvalidOperationException iox)
            {
                TempData["error"] = "Sorry, we could not find the requested item. Please try again.|" + iox.Message;
            }
            //catch (SqlException sx)
            //{
            //    TempData["error"] = "Some error occured while connecting to database. Please try again after sometime.|" + sx.Message;
            //}
            catch (NullReferenceException nx)
            {
                TempData["error"] = "Some required data was missing. Please try again.|" + nx.Message;
            }
            catch (Exception ex)
            {
                TempData["error"] = "Some error occured. Please try again.|" + ex.Message;
            }

            TempData["success"] = "";
            return RedirectToAction("SubContratorList");
        }

        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";

                       {

                try
                {
                    var ref_no = new SubContractorRepository().DeleteSubContractor(Id);
                    TempData["success"] = "Deleted Successfully (" + ref_no + ")";
                 return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Oops! Something went wrong!";
                    TempData["SubRefNo"] = null;
                    return RedirectToAction("Edit", new { id = Id });
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(SubContractor model)
        {
            //ViewBag.Title = "Update";

            {

                try
                {
                    var ref_no = new SubContractorRepository().UpdateSubContractor(model);
                    TempData["success"] = "Updated Successfully (" + ref_no + ")";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Oops! Something went wrong!";
                    TempData["SubRefNo"] = null;
                    return RedirectToAction("Edit", new { id = model.SubContractorId });
                }
            }
        }


    }
    }
