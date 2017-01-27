using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreationsPMSDomain
{
  public  class WorkStatus
    {
       public string ProjectEnquiry { get; set; }
        public string TaskName { get; set; }
        public int ProjectWorkId { get; set; }
        public string projectWorkRefNo { get; set; }
        public DateTime projectWorkDate { get; set; }
        public string MileStoneName { get; set; }
       public string ProjectWorkDescription { get; set; }
        public string SubName { get; set; }
        public decimal WorkAmount { get; set; }
        public decimal PercentageComplete { get; set; }
        public string Remarks { get; set; }
        public int ProjectWorkDetailsId { get; set; }
        public int ProjectWorkItemId { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<WorkStatusItem> WorkStatusItem { get; set; }
    }
    public class WorkStatusItem
    {
        public int ProjectWorkId { get; set; }
        public string projectWorkRefNo { get; set; }
        public DateTime projectWorkDate { get; set; }
        public string MileStoneName { get; set; }
        public string TaskName { get; set; }
        public string ProjectWorkDescription { get; set; }
        public string SubName { get; set; }
        public decimal WorkAmount { get; set; }
        public decimal PercentageComplete { get; set; }
        public string Remarks { get; set; }
        public int ProjectWorkDetailsId { get; set; }
        public int ProjectWorkItemId { get; set; }
    }
}
