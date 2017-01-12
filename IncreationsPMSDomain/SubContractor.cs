using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
    public class SubContractor
    {
        public SubContractor()
        {

        }
        public int SubContractorId { get; set; }
        public string SubRefNo { get; set; }
        //[Required]

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Subcontracor Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Subcontracor Name")]
        public string SubName { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public bool isUsed { get; set; }
    }
    }
