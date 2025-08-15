using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SignUpCompany.Data;
using SignUpCompany.Repository;
using SignUpCompany.Services.DTOs;


namespace SignUpCompany.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IOTPService _otpService;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CompanyService(ICompanyRepository companyRepository, IFileStorageService fileStorageService, IOTPService otpService, IEmailService emailService, JwtSettings jwtSettings, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _companyRepository = companyRepository;
            _fileStorageService = fileStorageService;
            _otpService = otpService;
            _emailService = emailService;
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool IsSuccess, string Message)> RegisterCompany(SignUpCompanyDTO companyDTO)
        {
            if (await _companyRepository.IsExist(c => c.Email == companyDTO.Email))
                return (false, "Email is already used.");

            if (!string.IsNullOrEmpty(companyDTO.PhoneNumber) && await _companyRepository.IsExist(c => c.PhoneNumber == companyDTO.PhoneNumber))
                return (false, "Phone Number is already used.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = companyDTO.Email,
                Email = companyDTO.Email,
                PhoneNumber = companyDTO.PhoneNumber,
                EmailConfirmed = false,
            };

            var userRegisteration = await _userManager.CreateAsync(user);
            if (!userRegisteration.Succeeded)
                return (false, userRegisteration.Errors.First().Description);

            var company = new Company
            {
                Id = user.Id,
                ArabicName = companyDTO.ArabicName,
                EnglishName = companyDTO.EnglishName,
                Email = companyDTO.Email,
                PhoneNumber = companyDTO.PhoneNumber,
                WebsiteUrl = companyDTO.WebsiteUrl,
                LogoFileName = null,
            };


            await _companyRepository.AddCompany(company);
            await _companyRepository.SaveChangesAsync();

            var otp = await _otpService.GenerateOTP(user.Id);

            await _emailService.SendOTPEmail(user.Email, otp);

            return (true, "Company registered successfully. Check your mail");
        }

        public async Task<(bool IsVerified, string Message)> VerifyCompany(VerifyOTPDTO verifyOTPDTO)
        {
            var user = await _userManager.Users
                 .Include(u => u.Company)
                 .FirstOrDefaultAsync(u => u.Email == verifyOTPDTO.Email);

            if (user == null)
                return (false, "User not Found.");

            if (user.EmailConfirmed)
                return (false, "Already verified.");

            var otpValidResult = await _otpService.VerifyOTP(user.Id, verifyOTPDTO.OTP);

            if (otpValidResult == OTPValidationResult.NotFound)
                return (false, "Invalid OTP.");

            if (otpValidResult == OTPValidationResult.Used)
                return (false, "OTP already used.");

            if (otpValidResult == OTPValidationResult.Expired)
                return (false, "OTP already Expired.");

            if (otpValidResult == OTPValidationResult.Incorrect)
                return (false, "Incorrect OTP.");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return (true, "Your account has been verified");
        }
        public async Task<(bool IsVerified, string Message)> ResendOTP(ResendOTP resendOTP)
        {
            var user = await _userManager.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Email == resendOTP.Email);

            if (user == null)
                return (false, "User not found.");

            if (user.EmailConfirmed)
                return (false, "Email already verified.");

            var newOtp = await _otpService.GenerateOTP(user.Id);

            await _emailService.SendOTPEmail(user.Email, newOtp);

            return (true, "A new OTP has been sent to your email. Please check it.");
        }
        public async Task<(bool IsSet, string Message)> SetPassword(SetPasswordDTO setPasswordDTO)
        {
            if (setPasswordDTO.Password != setPasswordDTO.ConfirmPassword)
                return (false, "Password do not match");

            var user = await _userManager.FindByEmailAsync(setPasswordDTO.Email);
            if (user == null)
                return (false, "User not Found.");

            if (!user.EmailConfirmed)
                return (false, "Email is not verified.");

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
                return (false, "Password already set");

            var result = await _userManager.AddPasswordAsync(user, setPasswordDTO.Password);
            if (!result.Succeeded)
                return (false, result.Errors.First().Description);

            return (true, "Password set successfully.");
        }
        public async Task<(bool IsSuccess, string Message, AuthDTO? Data)> SignIn(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByEmailAsync(signInDTO.Email);

            if (user == null)
                return (false, "Account is not exist.", null);

            if (!user.EmailConfirmed)
                return (false, "Account is no verified.", null);
            if (user.PasswordHash == null)
                return (false, "You need to set password", null);

            var isValidPassword = await _userManager.CheckPasswordAsync(user, signInDTO.Password);

            if (!isValidPassword)
                return (false, "Invalid email or password.", null);

            var company = await _companyRepository.GetById(user.Id);

            var Token = GenerateJwtToken(user, company);

            return (true, "Sign in Successfully", new AuthDTO { Token = Token });
        }
        public async Task<(bool IsSuccess, string Message, CompanyDTO? Data)> GetCompany(string email)
        {
            var company = await _companyRepository.GetByEmail(email);

            if (company == null)
                return (false, "Invalid email.", null);

            return (true, "Company retrieved successfully", company.CompanyToCompanyDTO());
        }
        public async Task<(bool IsSuccess, string Message)> UploadCompanyLogoAsync(Guid companyId, IFormFile logo)
        {
            if (logo == null || logo.Length == 0)
                return (false, "Logo file is required");

            var company = await _companyRepository.GetById(companyId);
            if (company == null)
                return (false, "Company not found.");

            var fileName = await _fileStorageService.SaveImageAsync(logo);
            if (string.IsNullOrEmpty(fileName))
                return (false, "Failed to save logo");

            company.LogoFileName = fileName;
            _companyRepository.UpdateCompany(company);
            await _companyRepository.SaveChangesAsync();

            return (true, "Logo saved successfully.");
        }

        #region Helpers
        public string GenerateJwtToken(User user, Company? company)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (company is not null)
            {
                claims.Add(new Claim("CompanyEnglishName", company.EnglishName));
                claims.Add(new Claim("CompanyArabicName", company.ArabicName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
