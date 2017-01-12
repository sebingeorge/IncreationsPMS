using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace IncreationsPMSDomain
{
 public   class ProjectTask
    {
        public int? ProjectTaskId { get; set; }
        public string MileStoneName { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       public int ProjectId { get; set; }
    }
}
