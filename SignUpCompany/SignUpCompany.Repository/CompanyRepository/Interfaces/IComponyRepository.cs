using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SignUpCompany.Data;

namespace SignUpCompany.Repository
{
    public interface ICompanyRepository
    {
        Task AddCompany(Company company);
        void UpdateCompany(Company company);
        Task<Company> GetById(Guid id);
        Task<Company> GetByEmail(string email);
        Task<bool> IsExist(Expression<Func<Company,bool>> predicate);
        Task SaveChangesAsync();
    }
}
