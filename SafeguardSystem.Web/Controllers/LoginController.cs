using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _loginService;

        public LoginController(IAuthService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _loginService.LoginAsync(loginDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login/signin-google")]
        public async Task<IActionResult> SignInWithGoogle([FromBody] GoogleLoginDTO googleLoginDTO)
        {
            var response = await _loginService.SignInWithGoogleAsync(googleLoginDTO);
            return StatusCode(response.StatusCode, response);
        }
    }
}
