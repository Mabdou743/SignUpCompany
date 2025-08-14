using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SignUpCompany.Data
{
    public class User : IdentityUser<Guid>
    {
        public Company? Company { get; set; }
        public ICollection<OTP> OTPs { get; set; }
    }
}
