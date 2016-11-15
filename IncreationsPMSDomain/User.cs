using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreationsPMSDomain
{
    public class User
    {
        public int? UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string UserSalt { get; set; }
        public int? UserRole { get; set; }
        public string CreatedBy { get; set; }
    }
}
