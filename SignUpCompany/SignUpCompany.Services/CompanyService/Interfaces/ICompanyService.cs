using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SignUpCompany.Data;
using SignUpCompany.Services.DTOs;

namespace SignUpCompany.Services
{
    public interface ICompanyService
    {
        Task<(bool IsSuccess, string Message)> RegisterCompany(SignUpCompanyDTO companyDTO);
        Task<(bool IsVerified, string Message)> VerifyCompany(VerifyOTPDTO verifyOTPDTO);
        Task<(bool IsVerified, string Message)> ResendOTP(ResendOTP resendOTP);
        Task<(bool IsSet, string Message)> SetPassword(SetPasswordDTO setPasswordDTO);
        Task<(bool IsSuccess, string Message, AuthDTO? Data)> SignIn(SignInDTO signInDTO);
        Task<(bool IsSuccess, string Message, CompanyDTO? Data)> GetCompany(string email);
        Task<(bool IsSuccess, string Message)> UploadCompanyLogoAsync(Guid companyId, IFormFile logo);

    }
}
