using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignUpCompany.Data;
using SignUpCompany.Services.DTOs;

namespace SignUpCompany.Services
{
    public static class CompanyExtensions
    {
        public static CompanyDTO CompanyToCompanyDTO(this Company company)
        {
            return new CompanyDTO
            {
                ArabicName = company.ArabicName,
                EnglishName = company.EnglishName,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                WebsiteUrl = company.WebsiteUrl,
                LogoFileName = company.LogoFileName
            };
        }
    }
}
