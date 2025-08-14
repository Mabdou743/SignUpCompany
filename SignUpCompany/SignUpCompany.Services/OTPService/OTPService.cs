using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignUpCompany.Data;
using SignUpCompany.Repository;

namespace SignUpCompany.Services.OTPService
{
    public class OTPService : IOTPService
    {
        private readonly IOTPRepository _otpRepository;
        public OTPService(IOTPRepository otpRepository) 
        { 
            _otpRepository = otpRepository;
        }
        public async Task<string> GenerateOTP(Guid userId)
        {
            var otp = new OTP
            {
                OtpCode = new Random().Next(100000, 999999).ToString(),
                ExpirationTime = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
                UserId = userId
            };

            await _otpRepository.Add(otp);
            await _otpRepository.SaveChanges();

            return otp.OtpCode;
        }

        public async Task<OTPValidationResult> VerifyOTP(Guid userId, string otpcode)
        {
            var otp = await _otpRepository.GetOtpByUserId(userId);
            
            if (otp == null) 
                return OTPValidationResult.NotFound;

            if(otp.IsUsed)
                return OTPValidationResult.Used;

            if (otp.ExpirationTime < DateTime.UtcNow)
                return OTPValidationResult.Expired;

            if (otp.OtpCode != otpcode)
                return OTPValidationResult.Incorrect;

            otp.IsUsed = true;
            await _otpRepository.Update(otp);
            await _otpRepository.SaveChanges();

            return OTPValidationResult.Success;
        }
    }
}
