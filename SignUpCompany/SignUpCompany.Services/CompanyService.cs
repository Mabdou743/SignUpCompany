using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SignUpCompany.Data;
using SignUpCompany.Repository;
using SignUpCompany.Services.DTOs;
using SignUpCompany.Services.Interfaces;

namespace SignUpCompany.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IEmailService emailService;
        private readonly JwtSettings jwtSettings;
        private readonly IWebHostEnvironment env;

        public CompanyService(ICompanyRepository _companyRepository, IEmailService _emailService, JwtSettings _jwtSettings, IWebHostEnvironment _env)
        {
            companyRepository = _companyRepository;
            emailService = _emailService;
            jwtSettings = _jwtSettings;
            env = _env;
        }

        public async Task<(bool IsSuccess, string Message)> RegisterCompany(SignUpCompanyDTO companyDTO)
        {
            if (await companyRepository.IsExist(c=>c.Email==companyDTO.Email))
                return (false, "Email is already used.");

            if (await companyRepository.IsExist(c => c.PhoneNumber == companyDTO.PhoneNumber))
                return (false, "Phone Number is already used.");

            string? logoFileName = await SaveImage(companyDTO.Logo);

            var company = new Company
            {
                ArabicName = companyDTO.ArabicName,
                EnglishName = companyDTO.EnglishName,
                Email = companyDTO.Email,
                PhoneNumber = companyDTO.PhoneNumber,
                WebsiteUrl = companyDTO.WebsiteUrl,
                LogoFileName = logoFileName,
                IsVerified = false,
                OtpCode = GenerateOTP()
            };


            await companyRepository.AddCompany(company);
            await companyRepository.SaveChangesAsync();

            await emailService.SendOTPEmail(company.Email, company.OtpCode);

            return (true, "Company registered successfully. Check your mail");
        }

        public async Task<(bool IsVerified, string Message)> VerifyCompany(VerifyOTPDTO verifyOTPDTO)
        {
            var company = await companyRepository.GetByEmail(verifyOTPDTO.Email);

            if (company == null) 
                return (false, "Company not Found");

            if (company.OtpCode != verifyOTPDTO.OTP)
                return (false, "Invalid OTP");

            company.IsVerified = true;
            await companyRepository.SaveChangesAsync();

            return (true, "Your company has been verified");
        }

        public async Task<(bool IsSet, string Message)> SetPassword(SetPasswordDTO setPasswordDTO)
        {
            var company = await companyRepository.GetByEmail(setPasswordDTO.Email);
            if (company == null)
                return (false, "Company not Found");

            var HashedPassword = BCrypt.Net.BCrypt.HashPassword(setPasswordDTO.Password);

            company.PasswordHash = HashedPassword;
            await companyRepository.SaveChangesAsync();

            return (true, "Password set successfully.");
        }

        public async Task<(bool IsSuccess, string Message, AuthDTO? Data)> SignIn(SignInDTO signInDTO)
        {
            var company = await companyRepository.GetByEmail(signInDTO.Email);

            if (company == null)
                return (false, "Your company not Exist Go sign Up.",null);

            if (!company.IsVerified)
                return (false,"Your account needs verifiy.", null);

            if (company.PasswordHash == null)
                return (false, "You need to set password.", null);

            bool validPassword = BCrypt.Net.BCrypt.Verify(signInDTO.Password, company.PasswordHash);
            if(!validPassword)
                return (false, "Invalid email or password.", null);

            var Token = GenerateJwtToken(company);
            var Data = new AuthDTO
            {
                Token = Token,
            };

            return (true, "Sign in Successfully", Data);
        }
        public async Task<(bool IsSuccess, string Message, CompanyDTO? Data)> GetCompany(string email)
        {
            var company = await companyRepository.GetByEmail(email);

            if (company == null)
                return (false, "Invalid email.", null);

            return (true, "Company retrieved successfully", company.CompanyToCompanyDTO());
        }

        #region Helpers
        private async Task<string> SaveImage(IFormFile logo)
        {
            if (logo != null && logo.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(logo.FileName)}";
                var logosFolder = Path.Combine(env.WebRootPath, "Logos");
                Directory.CreateDirectory(logosFolder);

                var filePath = Path.Combine(logosFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logo.CopyToAsync(stream);
                }

                return fileName;

            }
            return null;
        }

        public string GenerateOTP()
        {
            var OTP = new Random();
            return OTP.Next(100000, 999999).ToString();
        }

        public string GenerateJwtToken(Company company) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, company.Id.ToString()),
                new Claim(ClaimTypes.Email, company.Email),
                new Claim("CompanyEnglishName",company.EnglishName),
                new Claim("CompanyArabicName",company.ArabicName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
