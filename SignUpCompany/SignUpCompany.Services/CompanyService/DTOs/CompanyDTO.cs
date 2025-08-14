using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignUpCompany.Services.DTOs
{
    public class CompanyDTO
    {
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoFileName { get; set; }
    }
}
