using IncreationsPMSDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDomain;

namespace IncreationsPMSWeb.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult CustomerList(string Customer = "")
        {
            //List<Client> lstCustomer = (new CustomerRepository()).GetCustomer();
            //return PartialView("_CustomerList",lstCustomer);
            return PartialView("_CustomerList", new CustomerRepository().GetCustomer(Customer));
        }

        public ActionResult Create()
        {
            FillDropdowns();
            return View(new Client());
        }
        public ActionResult Edit(int Id)
        {
            ViewBag.Title = "Edit";
            FillDropdowns();
            Client objCustomer = new CustomerRepository().GetCustomer(Id);
            //ViewBag.UserRole = new SelectList((new UserRepository()).GetUserRole(), "RoleId", "RoleName", user.UserRole);
            //////ViewBag.District = new SelectList((new DistrictRepository()).GetDistrict(), "DistrictId", "DistrictName", objCustomer.District);
            return View("Create", objCustomer);

        }
        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";
            FillDropdowns();
            Client objCustomer = new CustomerRepository().GetCustomer(Id);
            return View("Create", objCustomer);
        }
        [HttpPost]
        public ActionResult Create(Client model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns();
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            model.CreatedBy = UserID.ToString();
            model.OrganizationId = OrganizationId;
            
            Result res = new CustomerRepository().Insert(model);
            if (res.Value)
            {
                TempData["success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Create");
        }
          [HttpPost]
        public ActionResult Edit(Client model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns();
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new CustomerRepository().Update(model);


            if (res.Value)
            {
                TempData["Success"] = "Updated Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Client model)
        {
            Result res = new CustomerRepository().Delete(model);
            if (res.Value)
            {
                TempData["Success"] = "Deleted Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        void FillDropdowns()
        {
            FillDistrict();

        }
        void FillDistrict()
        {
            ViewBag.District = new SelectList((new DistrictRepository()).GetDistrict(), "DistrictId", "DistrictName");
        }
    }



}