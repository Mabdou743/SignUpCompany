using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SignUpCompany.Data
{
    public class CompanyDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<OTP> OTPs { get; set; }

        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        }
    }
}
