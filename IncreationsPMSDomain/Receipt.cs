using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
    public class Receipt
    {
        //Pending Receipt
        public int CustInvoiceId { get; set; }
        public string CustInvoiceRefNo { get; set; }
        public DateTime CustInvoiceDate { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public decimal InvoiceAmount { get; set; }

        //Receipt
       public int ReceiptId { get; set; }
       public string ReceiptRefNo { get; set; }
       public DateTime ReceiptDate { get; set; }
       public int ClientId { get; set; }
       public decimal ReceivedAmount { get; set; }
       public decimal ReceivableAmount { get; set; }
       public int PaymentModeId { get; set; }
       public string ChequeNo { get; set; }
       public string VoucherNo { get; set; }
       public string SpecialRemarks { get; set; }
       public string CreatedBy { get; set; }
       public DateTime  CreatedDate { get; set; }

    }
}
