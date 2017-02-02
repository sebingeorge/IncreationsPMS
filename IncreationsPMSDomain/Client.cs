using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
    public class Client
    {
        public int? ClientId { get; set; }
    
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Client Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Client Name")]
        public string ClientName { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Client ShortName is Required")]
         [System.ComponentModel.DataAnnotations.Display(Name = "Client Short Name")]
        public string ClientShortName { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PostalCode)]
        public string Pin { get; set; }
        [Required]
        public int District { get; set; }
        public string Street { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CreditPeriod { get; set; }
     
        public Decimal CreditLimit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrganizationId { get; set; }
     

    }
}

