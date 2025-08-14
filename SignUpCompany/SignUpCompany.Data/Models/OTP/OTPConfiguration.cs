using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SignUpCompany.Data
{
    public class OTPConfiguration
    {
        public void Configure(EntityTypeBuilder<OTP> builder)
        {
            builder.HasKey(o=>o.Id);

            builder.Property(o => o.OtpCode)
               .IsRequired()
               .HasMaxLength(6);

            builder.Property(o => o.ExpirationTime)
               .IsRequired();


            builder.HasOne(o => o.User)
               .WithMany(u => u.OTPs)
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
