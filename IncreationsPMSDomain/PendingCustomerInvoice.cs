using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
   public class PendingCustomerInvoice
    {
        public int ProjectId { get; set; }
        public string ProjectRefNo { get; set; }
        public DateTime ProjectDate { get; set; }
        public string ProjectEnquiry { get; set; }
        public int Paymentid { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ClientName { get; set; }
    }
}
