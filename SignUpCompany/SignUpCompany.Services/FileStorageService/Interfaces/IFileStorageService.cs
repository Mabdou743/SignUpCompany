using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SignUpCompany.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveImageAsync(IFormFile Image);
    }
}
