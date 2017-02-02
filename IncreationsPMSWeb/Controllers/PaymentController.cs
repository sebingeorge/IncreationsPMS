using IncreationsPMSDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncreationsPMSDomain;
using System.Collections;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
//using IncreationsPMSWeb.Models;
using System.Data;
using System.Data.SqlClient;

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
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;
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
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;
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
        public ActionResult paymentFollowupPartial(string ProjectEnquiry = "", string ClientName = "")
        {
            return PartialView("_paymentFollowup", new CustomerInvoiceRepository ().PendingCustomerInvoice(ProjectEnquiry, ClientName));
        }

         public ActionResult paymentReport(int Id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "paymentReport.rpt"));

            DataSet ds = new DataSet();
            ds.Tables.Add("Head");
                      
            #region creating Head table
            ds.Tables["Head"].Columns.Add("PaymentRefNo");
            ds.Tables["Head"].Columns.Add("PaymentDate");
            ds.Tables["Head"].Columns.Add("SubName");
            ds.Tables["Head"].Columns.Add("WorkAmount");
            ds.Tables["Head"].Columns.Add("AcceptedAmount");
            ds.Tables["Head"].Columns.Add("PaymentModeName");
            ds.Tables["Head"].Columns.Add("ChequeNo");
            ds.Tables["Head"].Columns.Add("SpecialRemarks");
            #endregion
            
            #region store data to Head table
            PaymentRepository repo = new PaymentRepository();
            var Head = repo.PaymentPrint(Id);
            DataRow dr = ds.Tables["Head"].NewRow();
            dr["PaymentRefNo"] = Head.PaymentRefNo;
            dr["PaymentDate"] = Head.PaymentDate.ToString("dd-MMM-yyyy");
            dr["SubName"] = Head.SubName;
            dr["WorkAmount"] = Head.WorkAmount;
            dr["AcceptedAmount"] = Head.AcceptedAmount;
            dr["PaymentModeName"] = Head.PaymentModeName;
            dr["ChequeNo"] = Head.ChequeNo;
            dr["SpecialRemarks"] = Head.SpecialRemarks;
           ds.Tables["Head"].Rows.Add(dr);
            #endregion
            
            ds.WriteXml(Path.Combine(Server.MapPath("~/XML"), "paymentReport.xml"), XmlWriteMode.WriteSchema);

            rd.SetDataSource(ds);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}