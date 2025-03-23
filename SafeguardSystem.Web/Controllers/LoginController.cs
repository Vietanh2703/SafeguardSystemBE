using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    [SwaggerOperation(Summary = "User login", Description = "Authenticates a user with their account and password.")]
    [SwaggerResponse(200, "Login successful", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid login request")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var response = await _authService.LoginAsync(loginDTO);
        return StatusCode(response.StatusCode, response);
    }

    [Route("google")]
    [HttpPost]
    [SwaggerOperation(Summary = "Google sign-in", Description = "Authenticates a user using their Google account.")]
    [SwaggerResponse(200, "Sign-in successful", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid sign-in request")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> SignInWithGoogle([FromBody] GoogleLoginDTO googleLoginDTO)
    {
        var result = await _authService.SignInWithGoogleAsync(googleLoginDTO);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("logout")]
    [SwaggerOperation(Summary = "User logout", Description = "Logs out a user by invalidating their refresh token.")]
    [SwaggerResponse(200, "Logout successful", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid logout request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        var response = await _authService.LogoutAsync(refreshToken);
        return StatusCode(response.StatusCode, response);
    }
}