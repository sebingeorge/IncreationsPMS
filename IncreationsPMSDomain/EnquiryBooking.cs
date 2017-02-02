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
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Client Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Client Name")]
        public string  EnquiryClient { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Client Type is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Client Type")]
        public int? ClientTypeId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Mode Of Contact is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Mode Of Contact")]
        public int? ModeofContactId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Project Type is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Project Type")]
        public int? ProjectTypeId { get; set; }
        public string EnquiryReference { get; set; }
        public string   EnquiryContactNo { get; set; }
        public string EnquiryEmail { get; set; }
        public string EnquiryLocation { get; set; }
        public string EnquiryDetails { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrganizationId { get; set; }
    }
}
