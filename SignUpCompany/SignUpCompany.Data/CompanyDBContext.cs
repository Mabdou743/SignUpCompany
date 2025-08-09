using Microsoft.EntityFrameworkCore;

namespace SignUpCompany.Data
{
    public class CompanyDBContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
