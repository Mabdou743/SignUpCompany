using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SignUpCompany.Services
{
    public class FileStorageService : IFileStorageService
    {
        public readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var logosFolder = Path.Combine(_env.WebRootPath, "Images");
                Directory.CreateDirectory(logosFolder);

                var filePath = Path.Combine(logosFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                return fileName;
            }
            return null;
        }
    }
}
