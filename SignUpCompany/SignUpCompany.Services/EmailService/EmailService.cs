using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SignUpCompany.Services;

namespace SignUpCompany.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;


        public EmailService (IConfiguration _configuration)
        {
            configuration = _configuration;
        }


        public async Task SendOTPEmail(string email, string otp)
        {
            var smtpHost = configuration["SmtpSettings:Host"];
            var smtpPort = int.Parse(configuration["SmtpSettings:Port"]);
            var enableSSL = bool.Parse(configuration["SmtpSettings:EnableSSL"]);
            var smtpUser = configuration["SmtpSettings:UserName"];
            var smtpPass = configuration["SmtpSettings:Password"];

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = enableSSL
            };

            var mail = new MailMessage(smtpUser, email)
            {
                Subject = "Company OTP Verification",
                Body = $"Your OTP Code is: {otp}",
                IsBodyHtml = false
            };

            await smtpClient.SendMailAsync(mail);
        }
    }
}
