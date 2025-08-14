using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignUpCompany.Services.DTOs
{
    public class SignInDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{7,}$",
        ErrorMessage = "Password should contain at least one capital letter, special character, number, and be more than 6 characters.")]
        public string Password { get; set; }
    }
}
