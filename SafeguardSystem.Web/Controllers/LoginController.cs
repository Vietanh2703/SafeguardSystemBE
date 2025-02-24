using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.BLL.Services;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login/signin-google")]
        public async Task<IActionResult> SignInWithGoogle([FromBody] GoogleLoginDTO googleLoginDTO)
        {
            var response = await _authService.SignInWithGoogleAsync(googleLoginDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            var response = await _authService.LogoutAsync(refreshToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
