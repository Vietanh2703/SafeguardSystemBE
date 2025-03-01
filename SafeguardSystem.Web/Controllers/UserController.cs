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

        [Route("verify-email")]
        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpDTO otpDTO)
        {
            var results = await _userService.VerifyOtpAsync(otpDTO.Email,otpDTO);
            return StatusCode(results.StatusCode, results);
        }

        [Route("update-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var result = await _userService.UpdateUserAsync(userId, updateUserDTO);
            return StatusCode(result.StatusCode, result);
        }
    }
}
