using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignUpCompany.Data;

namespace SignUpCompany.Repository
{
    public interface IOTPRepository
    {
        Task<OTP?> GetOtpByUserId(Guid userId);
        Task Add(OTP otp);
        Task Update(OTP otp);
        Task SaveChanges();
    }
}
