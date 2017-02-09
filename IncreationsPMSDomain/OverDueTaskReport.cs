using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreationsPMSDomain
{
  public   class OverDueTaskReport
    {
        public string ProjectRefNo { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public string ProjectEnquiry { get; set; }
        public int ProjectTaskId { get; set; }
        public string TaskName { get; set; }
        public decimal PercentageComplete { get; set; }
        public string ProjectWorkDescription { get; set; }
        public  string DiffDate { get; set; }
    }
}
