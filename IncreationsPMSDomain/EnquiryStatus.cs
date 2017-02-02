using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
  public   class EnquiryStatus
    {
        public int EnquiryId { get; set; }
        public string EnquiryRef { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryClient { get; set; }
        public string ClientTypeName { get; set; }
         public string ProjectTypeName { get; set; }
        public string EnquiryReference { get; set; }
        public string EnquiryContactNo { get; set; }
        public string EnquiryEmail { get; set; }
        public string EnquiryLocation { get; set; }
        public string EnquiryDetails { get; set; }
        public Boolean EnquiryProfileSending { get; set; }
        public Boolean EnquiryOfferSending { get; set; }
        public Boolean EnquiryLayoutReceiving { get; set; }
       
 }
}
