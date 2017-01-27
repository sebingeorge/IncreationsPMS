using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
  public  class BOQPreparing
    {
        public int ProjectWorkId { get; set; }
        public string projectWorkRefNo { get; set; }
        public DateTime projectWorkDate { get; set; }
        public string  PreparedBy { get; set; }
        public int ProjectId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ProjectEnquiry { get; set; }
        public List<BOQPreparingItem> BOQPreparingItem { get; set; }
      
    }
    public class BOQPreparingItem
    {
        public int ProjectWorkItemId { get; set; }
        public int ProjectWorkId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectRefNo { get; set; }
        public DateTime ProjectDate { get; set; }
        public int? ProjectTaskId { get; set; }
        public string MileStoneName { get; set; }
        public string TaskName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<BOQPreparingItemWork> BOQPreparingItemWork { get; set; }

    }

    public class BOQPreparingItemWork
    {
        public int ProjectWorkId { get; set; }
        public int ProjectWorkDetailsId { get; set; }
        public int ProjectTaskId { get; set; }
        public int ProjectWorkItemId { get; set; }
        public string ProjectWorkDescription { get; set; }
        public int SubContractorId { get; set; }
        public string SubName { get; set; }
        public DateTime PlanedStartDate { get; set; }
        public DateTime PlanedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal WorkAmount { get; set; }
        public decimal PercentageComplete { get; set; }
        public string Remarks { get; set; }
        public int sno { get; set; }
    }
    }
