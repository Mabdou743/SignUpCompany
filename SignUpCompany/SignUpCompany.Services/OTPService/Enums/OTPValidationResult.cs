using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignUpCompany.Services
{
    public enum OTPValidationResult
    {
        Success,
        Expired,
        Used,
        Incorrect,
        NotFound

    }
}
