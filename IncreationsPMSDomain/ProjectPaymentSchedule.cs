using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace IncreationsPMSDomain
{
   public class ProjectPaymentSchedule
    {
        public int? Paymentid { get; set; }
        public int? ProjectId { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }

    }
}
