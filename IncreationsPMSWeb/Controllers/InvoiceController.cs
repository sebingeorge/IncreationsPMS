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
    public class InvoiceController : BaseController
    {
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Invoice(int Paymentid)
        {

            Invoice objInvoice = new CustomerInvoiceRepository().GetInvoice(Paymentid);
            var internalid = CustomerInvoiceRepository.GetNextRefNo();
            objInvoice.CustInvoiceRefNo = internalid;
            objInvoice.CustInvoiceDate = DateTime.Now;
            objInvoice.BillDueDate = DateTime.Now;
            objInvoice.CustomerInvoiceItem = new CustomerInvoiceRepository().GetInvoiceItem(Paymentid);
            return View("Invoice", objInvoice);
            //return View();
        }
        [HttpPost]
        public ActionResult Invoice(Invoice model)
        {
            model.CreatedBy = UserID.ToString();
            model.CreatedDate = System.DateTime.Now;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new CustomerInvoiceRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("paymentFollowup", "Payment");


        }
        public ActionResult PreviousInvoice()
        {
            ViewBag.Fromdate = FYStartdate;
            return View();
        }
        public ActionResult PendingProjects()
        {
            return View();
        }

        public ActionResult PreviousInvoicePartial( DateTime? FromDate, DateTime? ToDate, string ClientName = "")
        {
            FromDate = FromDate ?? FYStartdate;
            ToDate = ToDate ?? DateTime.Now;
            List<Invoice> obj = new List<Invoice>();
            obj = new CustomerInvoiceRepository().InvoiceList(FromDate,ToDate,ClientName);

            return PartialView("_PreviousInvoiceList", obj);
        }
        public ActionResult Update(int id)
        {
            try
            {
                if (id == 0)
                {
                    TempData["error"] = "That was an invalid/unknown request. Please try again.";
                    return RedirectToAction("Index", "Home");
                }
                var model = new CustomerInvoiceRepository().GetInvoiceView(id);
                if (model == null)
                {
                    TempData["error"] = "Could not find the requested Purchase Indent. Please try again.";
                    return RedirectToAction("Index", "Home");
                }
               
                return View("Invoice", model);
            }
            catch (Exception)
            {
                TempData["error"] = "Some error occured. Please try again.";
                return RedirectToAction("PreviousInvoice");
            }
        }
        [HttpPost]
        public ActionResult Update(Invoice model)
        {
            try
            {
                model.CreatedBy = UserID.ToString();
                model.CreatedDate = System.DateTime.Now;
                var success = new CustomerInvoiceRepository().UpdateInvoice(model);
                if (success <= 0) throw new Exception();
                TempData["success"] = "Updated successfully (" + model.CustInvoiceRefNo + ")";
                return RedirectToAction("PreviousInvoice");
            }
            catch (Exception)
            {
                TempData["error"] = "Some error occured while saving. Please try again.";
                return View("Invoice", model);
            }
        }
        public ActionResult DeleteInvoice(int id = 0)
        {
            try
            {
                if (id == 0) return RedirectToAction("Index", "Home");
                string result = new CustomerInvoiceRepository().DeleteInvoice(id);
                TempData["Success"] = "Deleted Successfully!";
                return RedirectToAction("PreviousInvoice");
            }
            catch (Exception)
            {
                TempData["error"] = "Some error occured while deleting. Please try again.";
                return RedirectToAction("Invoice", new { id = id });
            }
        }
        public ActionResult InvoiceReport(int Id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CustomerInvoice.rpt"));

            DataSet ds = new DataSet();
            ds.Tables.Add("Head");
            ds.Tables.Add("Items");
           
            #region creating Head table
            ds.Tables["Head"].Columns.Add("CustInvoiceRefNo");
            ds.Tables["Head"].Columns.Add("CustInvoiceDate");
            ds.Tables["Head"].Columns.Add("ClientName");
            ds.Tables["Head"].Columns.Add("ProjectOrderRefNo");
            ds.Tables["Head"].Columns.Add("Address1");
            ds.Tables["Head"].Columns.Add("Address2");
            ds.Tables["Head"].Columns.Add("Address3");
            ds.Tables["Head"].Columns.Add("SpecialRemarks");
            ds.Tables["Head"].Columns.Add("PaymentTerms");
            ds.Tables["Head"].Columns.Add("AdditionalRemarks");
            ds.Tables["Head"].Columns.Add("AddAmount");
            ds.Tables["Head"].Columns.Add("DeductionRemarks");
            ds.Tables["Head"].Columns.Add("DedAmount");
            ds.Tables["Head"].Columns.Add("BillDueDate");   
            #endregion

            #region creating Item Table
            ds.Tables["Items"].Columns.Add("ProjectRefNo");
            ds.Tables["Items"].Columns.Add("ProjectDate");
            ds.Tables["Items"].Columns.Add("ProjectEnquiry");
            ds.Tables["Items"].Columns.Add("Description");
            ds.Tables["Items"].Columns.Add("Amount");
            ds.Tables["Items"].Columns.Add("InvoiceAmount");
            #endregion


            #region store data to Head table
            CustomerInvoiceRepository repo = new CustomerInvoiceRepository();
            var Head = repo.CustomerInvoiceHdforPrint(Id);
            DataRow dr = ds.Tables["Head"].NewRow();
            dr["CustInvoiceRefNo"] = Head.CustInvoiceRefNo;
            dr["CustInvoiceDate"] = Head.CustInvoiceDate.ToString("dd-MMM-yyyy");
            dr["ClientName"] = Head.ClientName;
            dr["ProjectOrderRefNo"] = Head.ProjectOrderRefNo;
            dr["Address1"] = Head.Address1;
            dr["Address2"] = Head.Address2;
            dr["Address3"] = Head.Address3;
            dr["SpecialRemarks"] = Head.SpecialRemarks;
            dr["PaymentTerms"] = Head.PaymentTerms;
            dr["AdditionalRemarks"] = Head.AdditionalRemarks;
            dr["AddAmount"] = Head.AddAmount;
            dr["DeductionRemarks"] = Head.DeductionRemarks;
            dr["DedAmount"] = Head.DedAmount;
            dr["BillDueDate"] = Head.BillDueDate.ToString("dd-MMM-yyyy");
            ds.Tables["Head"].Rows.Add(dr);
            #endregion

            #region store data to Items Table
            CustomerInvoiceRepository repo1 = new CustomerInvoiceRepository();
            var Items = repo1.customerInvoiceItemforPrint(Id);
            foreach (var item in Items)
            {
                DataRow dri = ds.Tables["Items"].NewRow();
                dri["ProjectRefNo"] = item.ProjectRefNo;
                dri["ProjectDate"] = item.ProjectDate.ToString("dd-MMM-yyyy");
                dri["ProjectEnquiry"] = item.ProjectEnquiry;
                dri["Description"] = item.Description;
                dri["Amount"] = item.Amount;
                dri["InvoiceAmount"] = item.InvoiceAmount;
                ds.Tables["Items"].Rows.Add(dri);
            }
            #endregion

          

            ds.WriteXml(Path.Combine(Server.MapPath("~/XML"), "CustomerInvoice.xml"), XmlWriteMode.WriteSchema);

            rd.SetDataSource(ds);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");//, String.Format("SalesInvoice{0}.pdf", Id.ToString()));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}