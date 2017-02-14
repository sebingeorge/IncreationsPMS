using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
   public class PendingListForProject
    {
        public int EnquiryId { get; set; }
        public string EnquiryRef { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryClient { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
    }
}
