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

        [Route("roles")]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return StatusCode(roles.StatusCode, roles);
        }

        [Route("refresh-otp")]
        [HttpPost]
        public async Task<IActionResult> RefreshOtp(string email)
        {
            var result = await _userService.RefreshOtpAsync(email);
            return StatusCode(result.StatusCode, result);
        }

        [Route("verify-email")]
        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpDTO otpDTO)
        {
            var results = await _userService.VerifyOtpAsync(otpDTO.Email,otpDTO);
            return StatusCode(results.StatusCode, results);
        }

        [Route("update-password")]
        [HttpPut]
        public async Task<IActionResult> UpdatePassword(string Email,[FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            var result = await _userService.UpdatePasswordAsync(Email,updatePasswordDTO);
            return StatusCode(result.StatusCode, result);
        }

        [Route("update-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var result = await _userService.UpdateUserAsync(userId, updateUserDTO);
            return StatusCode(result.StatusCode, result);
        }

        [Route("user-details/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserByUserId(string userId)
        {
            var result = await _userService.GetUserByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
