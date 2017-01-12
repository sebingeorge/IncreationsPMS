using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
  public class EnquiryBooking
    {
        public int EnquiryId { get; set; }
        public string EnquiryRef { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string  EnquiryClient { get; set; }
        public int? ClientTypeId { get; set; }
        public int? ModeofContactId { get; set; }
        public int? ProjectTypeId { get; set; }
        public string EnquiryReference { get; set; }
        public string   EnquiryContactNo { get; set; }
        public string EnquiryEmail { get; set; }
        public string EnquiryLocation { get; set; }
        public string EnquiryDetails { get; set; }
    }
}
