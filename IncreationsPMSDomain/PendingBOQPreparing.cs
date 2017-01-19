using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
   public class PendingBOQPreparing
    {
        public int ProjectId { get; set; }
        public string ProjectRefNo { get; set; }
        public DateTime? ProjectDate { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ProjectEnquiry { get; set; }
      
    }
}
