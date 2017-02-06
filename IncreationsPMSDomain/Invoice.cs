using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
    public class Invoice
    {
        public int CustInvoiceId { get; set; }
        public string CustInvoiceRefNo { get; set; }
        public DateTime CustInvoiceDate { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ProjectOrderRefNo { get; set; }
        public string SpecialRemarks { get; set; }
        public string PaymentTerms { get; set; }
        public string AdditionalRemarks { get; set; }
        public decimal AddAmount { get; set; }
        public string DeductionRemarks { get; set; }
        public decimal DedAmount { get; set; }
        public DateTime BillDueDate { get; set; }
        public string ProjectEnquiry { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CustomerInvoiceItem> CustomerInvoiceItem { get; set; }

    }
    public class CustomerInvoiceItem
    {
        public int CustInvoiceItemId { get; set; }
        public int CustInvoiceId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectRefNo { get; set; }
        public DateTime ProjectDate { get; set; }
        public string ProjectEnquiry { get; set; }
        public int PaymentScheduleid { get; set; }
        public int Paymentid { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public decimal InvoiceAmount { get; set; }

    }
}
