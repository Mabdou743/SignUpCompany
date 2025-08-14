using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignUpCompany.Data;

namespace SignUpCompany.Services
{
    public interface IOTPService
    {
        public Task<string> GenerateOTP(Guid userId);

        public Task<OTPValidationResult> VerifyOTP(Guid userId, string otp);
    }
}
