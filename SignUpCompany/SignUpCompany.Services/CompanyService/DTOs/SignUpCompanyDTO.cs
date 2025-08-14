using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SignUpCompany.Services
{
    public class SignUpCompanyDTO
    {
        [Required]
        public string ArabicName { get; set; }
        [Required]
        public string EnglishName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(12)]
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
