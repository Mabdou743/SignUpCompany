using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignUpCompany.Data
{
    public class OTP
    {
        public int Id { get; set; }
        public string OtpCode { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
