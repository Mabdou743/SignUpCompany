using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignUpCompany.Data;

namespace SignUpCompany.Repository
{
    public class OTPRepository : IOTPRepository
    {
        private readonly CompanyDBContext _dbContext;
        public OTPRepository(CompanyDBContext companyDBContext) 
        {
            _dbContext = companyDBContext;
        }
        public async Task<OTP?> GetOtpByUserId(Guid userId)
        {
            return await _dbContext.OTPs
                .Where(o=>o.UserId == userId)
                .OrderByDescending(o => o.ExpirationTime)
                .FirstOrDefaultAsync();
        }
        public async Task Add(OTP otp)
        {
            await _dbContext.AddAsync(otp);

        }
        public async Task Update(OTP otp)
        {
            _dbContext.Update(otp);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
