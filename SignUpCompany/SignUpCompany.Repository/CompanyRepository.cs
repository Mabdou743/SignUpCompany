using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SignUpCompany.Data;

namespace SignUpCompany.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyDBContext dbContext;
        public CompanyRepository(CompanyDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task AddCompany(Company company)
        {
            await dbContext.Companies.AddAsync(company);
        }

        public async Task<Company> GetByEmail(string email)
        {
            return await dbContext.Companies.FirstOrDefaultAsync(c=>c.Email == email);
        }

        public async Task<Company> GetById(Guid id)
        {
            return await dbContext.Companies.FindAsync(id);
        }

        public async Task<bool> IsExist(Expression<Func<Company, bool>> predicate)
        {
            return await dbContext.Companies.AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
