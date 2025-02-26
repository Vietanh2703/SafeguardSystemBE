using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("verify-email")]
        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpDTO otpDTO)
        {
            var results = await _userService.VerifyOtpAsync(otpDTO);
            return StatusCode(results.StatusCode, results);
        }
    }
}
