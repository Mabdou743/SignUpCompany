using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SignUpCompany.Data
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c=>c.Id);

            builder.Property(c => c.EnglishName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.ArabicName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.PhoneNumber)
               .HasMaxLength(20);

            builder.Property(c => c.WebsiteUrl)
               .HasMaxLength(300);

            builder.Property(c => c.LogoFileName)
               .HasMaxLength(300);

            builder.Property(c => c.PasswordHash)
               .HasMaxLength(500);
        }
    }
}
