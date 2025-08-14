using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SignUpCompany.Services
{
    public interface IEmailService
    {
        public Task SendOTPEmail(string Email, string OTP);
    }
}
