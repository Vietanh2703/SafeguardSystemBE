using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers;

public class UserController : ControllerBase
{
    private readonly IAWSS3Service _awsS3Service;
    private readonly IGuardService _securityGuardService;
    private readonly IUserService _userService;

    public UserController(IUserService userService, IGuardService securityGuardService, IAWSS3Service aWSS3Service)
    {
        _userService = userService;
        _securityGuardService = securityGuardService;
        _awsS3Service = aWSS3Service;
    }

    [Route("roles")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all roles", Description = "Retrieve a list of all roles.")]
    [SwaggerResponse(200, "Successfully retrieved list of roles.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _userService.GetAllRolesAsync();
        return StatusCode(roles.StatusCode, roles);
    }

    [Route("refresh-otp/{email}")]
    [HttpPost]
    [SwaggerOperation(Summary = "Refresh OTP", Description = "Refresh the OTP for the given email.")]
    [SwaggerResponse(200, "OTP refreshed successfully.")]
    [SwaggerResponse(400, "Invalid email address.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> RefreshOtp(string email)
    {
        var result = await _userService.RefreshOtpAsync(email);
        return StatusCode(result.StatusCode, result);
    }

    [Route("email/{Email}")]
    [HttpPost]
    [SwaggerOperation(Summary = "Verify OTP", Description = "Verify the OTP for the given email.")]
    [SwaggerResponse(200, "OTP verified successfully.")]
    [SwaggerResponse(400, "Invalid OTP or email address.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> VerifyOtp(string Email, [FromBody] OtpDTO otpDTO)
    {
        var results = await _userService.VerifyOtpAsync(Email, otpDTO);
        return StatusCode(results.StatusCode, results);
    }

    [Route("password/{Email}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update password", Description = "Update the password for the given email.")]
    [SwaggerResponse(200, "Password updated successfully.")]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> UpdatePassword(string Email, [FromBody] UpdatePasswordDTO updatePasswordDTO)
    {
        var result = await _userService.UpdatePasswordAsync(Email, updatePasswordDTO);
        return StatusCode(result.StatusCode, result);
    }

    [Route("user/{userId}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update user details",
        Description = "Update the details of an existing user by their ID.")]
    [SwaggerResponse(200, "User details updated successfully.")]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(404, "User not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDTO updateUserDTO)
    {
        var result = await _userService.UpdateUserAsync(userId, updateUserDTO);
        return StatusCode(result.StatusCode, result);
    }

    [Route("user/{userId}")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get user details",
        Description = "Fetches the details of a user including the avatar URL.")]
    [SwaggerResponse(200, "User details retrieved successfully.", typeof(UserDTO))]
    [SwaggerResponse(404, "User not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetUserByUserId(string userId)
    {
        var result = await _userService.GetUserByUserIdAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [Route("{guardId}")]
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve security guard profile",
        Description = "Fetches the profile details of a security guard based on the provided guard ID.")]
    [SwaggerResponse(200, "Successfully retrieved security guard profile.")]
    [SwaggerResponse(400, "Invalid request or missing guard ID.")]
    [SwaggerResponse(404, "Security guard not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetGuardProfileById(Guid guardId)
    {
        var response = await _securityGuardService.GetGuardByIdAsync(guardId);
        return StatusCode(response.StatusCode, response);
    }

    [Route("{guardId}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update security guard profile",
        Description = "Updates the profile details of a security guard based on the provided guard ID.")]
    [SwaggerResponse(200, "Security guard profile updated successfully.")]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(404, "Security guard not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> UpdateGuardProfile(Guid guardId, [FromBody] SecurityGuardDTO guardDTO)
    {
        var response = await _securityGuardService.UpdateGuardAsync(guardId, guardDTO);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{userId}/avatar")]
    [SwaggerOperation(Summary = "View user avatar", Description = "Fetches the pre-signed URL for the user's avatar.")]
    [SwaggerResponse(200, "Pre-signed URL generated successfully.", typeof(string))]
    [SwaggerResponse(404, "User avatar not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> ViewAvatar(string userId)
    {
        // Fetch user details to get the avatar key
        var userResponse = await _userService.GetUserByUserIdAsync(userId);
        if (!userResponse.IsSuccess) return StatusCode(userResponse.StatusCode, userResponse.Message);

        var user = userResponse.Result as UserDTO;
        if (user == null || string.IsNullOrEmpty(user.Avatar)) return StatusCode(404, "User avatar not found.");

        // Use the avatar key directly from the user's avatar property
        var avatarKey = user.Avatar;
        var response = await _awsS3Service.GetPreSignedURLAsync(avatarKey);

        if (!response.IsSuccess) return StatusCode(response.StatusCode, response.Message);

        return Ok(response.Result.ToString());
    }
}