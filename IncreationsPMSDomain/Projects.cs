using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
    public class Projects
    {
        public int ProjectId { get; set; }
        public string ProjectRefNo { get; set; }
        public DateTime ProjectDate { get; set; }
        public int ClientId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ProjectEnquiry { get; set; }
        public List<ProjectTask> ProjectTask { get; set; }
        public List<ProjectPaymentSchedule> ProjectPaymentSchedule { get; set; }
    }
}
