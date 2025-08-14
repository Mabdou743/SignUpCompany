using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SignUpCompany.Data;

namespace SignUpCompany.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyDBContext _dbContext;
        public CompanyRepository(CompanyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCompany(Company company)
        {
            await _dbContext.Companies.AddAsync(company);
        }

        public void UpdateCompany(Company company)
        {
            _dbContext.Companies.Update(company);
        }

        public async Task<Company> GetByEmail(string email)
        {
            return await _dbContext.Companies.FirstOrDefaultAsync(c=>c.Email == email);
        }

        public async Task<Company> GetById(Guid id)
        {
            return await _dbContext.Companies.FindAsync(id);
        }

        public async Task<bool> IsExist(Expression<Func<Company, bool>> predicate)
        {
            return await _dbContext.Companies.AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
