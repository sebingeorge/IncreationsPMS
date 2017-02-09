using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreationsPMSDomain
{
   public  class AgeingSummary
    {
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public string ProjectEnquiry { get; set; }
        public decimal TotalReceivable { get; set; }
        public decimal Overdue { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount5 { get; set; }
        
        public DateTime Date { get; set; }
      
   
    }
}
