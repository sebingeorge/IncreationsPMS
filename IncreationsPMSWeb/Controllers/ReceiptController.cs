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
using System.Data;
using System.Data.SqlClient;


namespace IncreationsPMSWeb.Controllers
{
    public class ReceiptController : BaseController
    {
        // GET: Collecting
        public ActionResult Index()
        {
            ViewBag.Fromdate = FYStartdate;
            return View();
        }
        public ActionResult PendingReceiptPartial(DateTime? FromDate, DateTime? ToDate, string ClientName = "")
        {
            FromDate = FromDate ?? FYStartdate;
            ToDate = ToDate ?? DateTime.Now;
            List<Receipt> obj = new List<Receipt>();
            obj = new ReceiptRepository().PendingReceiptList(FromDate, ToDate, ClientName);

            return PartialView("_PendingReceipt", obj);
        }
        public ActionResult Receipt(int CustInvoiceId)
        {
            PaymentModeDropDown();
            Receipt objReceipt = new ReceiptRepository().GetNewReceipt(CustInvoiceId);
            var internalid = ReceiptRepository.GetNextRefNo();
            objReceipt.ReceiptRefNo = internalid;
            objReceipt.ReceiptDate = DateTime.Now;
            return View("Receipt", objReceipt);
        }
        public void PaymentModeDropDown()
        {
            ViewBag.PaymentModeList = new SelectList(new DropdownRepository().FillPaymentMode(), "Id", "Name");
        }
        [HttpPost]
        public ActionResult Receipt(Receipt model)
        {
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new ReceiptRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");

        }
        public ActionResult PreviousReceipt()
        {
            return View();
        }
        public ActionResult PreviousReceiptPartial(string ClientName = "")
        {
            List<Receipt> obj = new List<Receipt>();
            obj = new ReceiptRepository().ReceiptList(ClientName);
            return PartialView("_PreviousReceipt", obj);
        }
        public ActionResult Update(int id)
        {
            if (id == 0)
            {
                TempData["error"] = "That was an invalid/unknown request.";
                return RedirectToAction("Index", "Home");
            }
            ReceiptRepository repo = new ReceiptRepository();
            PaymentModeDropDown();

            Receipt model = repo.ReceiptView(id);
            return View("Receipt", model);
        }
        [HttpPost]
        public ActionResult Update(Receipt model)
        {
            //ViewBag.Title = "Update";
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;
            {

                try
                {
                    var ref_no = new ReceiptRepository().UpdateReceipt(model);
                    TempData["success"] = "Updated Successfully (" + ref_no + ")";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Oops! Something went wrong!";
                    TempData["ReceiptRefNo"] = null;
                    return RedirectToAction("Edit", new { id = model.ReceiptId });
                }
            }
        }
        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";

            {

                try
                {
                    var ref_no = new ReceiptRepository().DeleteReceipt(Id);
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
        public ActionResult ReceivableReport()
        {

            FillCustomer();
            return View();
        }
      
       public ActionResult ReceivableReportPartial(int ClientId = 0)
        {
          return PartialView("_ReceivableReport", new ReceiptRepository().GetReceiptReport(ClientId));
        }
        public void FillCustomer()
        {
            DropdownRepository repo = new DropdownRepository();
            var result = repo.GetClient();
            ViewBag.CustomerList = new SelectList(result, "Id", "Name");

        }
        public ActionResult Print(int ClientId = 0, string ClientName = "")
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "ReceivableReport.rpt"));

            DataSet ds = new DataSet();
            ds.Tables.Add("Items");

            //    //-------HEAD
                    

            //-------DT
            ds.Tables["Items"].Columns.Add("CustInvoiceRefNo");
            ds.Tables["Items"].Columns.Add("CustInvoiceDate");
            ds.Tables["Items"].Columns.Add("ClientName");
            ds.Tables["Items"].Columns.Add("Address");
            ds.Tables["Items"].Columns.Add("ReceivableAmount");
          
            
            ReceiptRepository repo1 = new ReceiptRepository();
             var Items = repo1.GetReceiptPrint(ClientId, ClientName);

            foreach (var item in Items)
            {
                DataRow dri = ds.Tables["Items"].NewRow();
                dri["CustInvoiceRefNo"] = item.CustInvoiceRefNo;
                dri["CustInvoiceDate"] = item.CustInvoiceDate.ToString("dd-MMM-yyyy");
                dri["ClientName"] = item.ClientName;
                dri["Address"] = item.Address;
                dri["ReceivableAmount"] = item.ReceivableAmount;
                ds.Tables["Items"].Rows.Add(dri);
            }

            ds.WriteXml(Path.Combine(Server.MapPath("~/XML"), "ReceivableReport.xml"), XmlWriteMode.WriteSchema);

            rd.SetDataSource(ds);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", String.Format("ReceivableReport.pdf"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}