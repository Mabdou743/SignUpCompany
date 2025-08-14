using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignUpCompany.Services.DTOs
{
    public class VerifyOTPDTO
    {
        [Required]
        public string Email { get; set; } 
        [Required]
        public string OTP { get; set; }
    }
}
