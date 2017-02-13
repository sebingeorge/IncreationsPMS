using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreationsPMSDomain
{
   public  class PendingPaymentSchedule
    {
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public string  ProjectEnquiry { get; set; }
        public DateTime date { get; set; }
        public string  Description { get; set; }
        public decimal   Amount { get; set; }
    }
}
