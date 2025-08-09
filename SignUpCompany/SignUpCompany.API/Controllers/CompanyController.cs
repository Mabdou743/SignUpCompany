using Microsoft.AspNetCore.Mvc;
using SignUpCompany.Services;
using SignUpCompany.Services.DTOs;

namespace SignUpCompany.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService _companyService)
        {
            companyService = _companyService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpCompany([FromForm] SignUpCompanyDTO signUpCompanyDTO)
        {
            var result = await companyService.RegisterCompany(signUpCompanyDTO);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<SignUpCompanyDTO>
                {
                    Status = 400,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponse<SignUpCompanyDTO>
            {
                Status = 200,
                Message = result.Message,
                Data = null
            });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPDTO verifyOTPDTO)
        {
            var result = await companyService.VerifyCompany(verifyOTPDTO);

            if (!result.IsVerified)
                return BadRequest(new ApiResponse<VerifyOTPDTO>
                {
                    Status = 400,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponse<VerifyOTPDTO>
            {
                Status = 200,
                Message = result.Message,
                Data = null
            });
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDTO setPassword)
        {
            var result = await companyService.SetPassword(setPassword);

            if (!result.IsSet)
                return BadRequest(new ApiResponse<SetPasswordDTO>
                {
                    Status = 400,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponse<SetPasswordDTO>
            {
                Status = 200,
                Message = result.Message,
                Data = null
            });
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInDTO signInDTO)
        {
            var result = await companyService.SignIn(signInDTO);

            if (!result.IsSuccess)
                return Unauthorized(new ApiResponse<AuthDTO>
                {
                    Status = 401,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponse<AuthDTO>
            {
                Status = 200,
                Message = result.Message,
                Data = result.Data
            });

        }

        [HttpGet("home")]
        public async Task<IActionResult> DataForHome(string email)
        {
            var result = await companyService.GetCompany(email);

            if (!result.IsSuccess)
                return Ok(new ApiResponse<CompanyDTO>
                {
                    Status = 400,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponse<CompanyDTO>
            {
                Status = 200,
                Message = result.Message,
                Data = result.Data
            });
        }
    }

}

