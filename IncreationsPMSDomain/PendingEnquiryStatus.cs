using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
  public  class PendingEnquiryStatus
    {
        public int EnquiryId { get; set; }
        public string EnquiryRef { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryClient { get; set; }
        public string ModeofContactName { get; set; }
        public string ProjectTypeName { get; set; }
        public int ProjectTypeId { get; set; }
        public int EnquiryStatusId { get; set; }
        public string EnquiryStatusName { get; set; }
    }
}
