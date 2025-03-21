using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.Common.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml.Linq;

namespace SafeguardSystem.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;
        private readonly ILoginRequestService _loginRequestService;

        public AdminController(IBusinessService businessService, IUserService userService, ILoginRequestService loginRequestService)
        {
            _businessService = businessService;
            _userService = userService;
            _loginRequestService = loginRequestService;
        }


        [Route("business-partners")]
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all business partners.",
                           Description = "This API returns a list of all registered business partner in the system.")]
        [SwaggerResponse(200, "Business partner retrieves successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public ResponseDTO GetAllBusinesses()
        {
            var results = _businessService.GetAllBusinessesAsync();
            return results;
        }

        [Route("user")]
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new user",
                           Description = "This API creates a new user based on the provided information.")]
        [SwaggerResponse(200, "User created successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            var results = await _userService.CreateUserAsync(createUserDTO);
            return StatusCode(results.StatusCode, results);
        }

        [Route("user-pagings")]
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all users with paging.",
                           Description = "This API returns all users that existed in system with paging.")]
        [SwaggerResponse(200, "User retrieves successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetAllUsers(int pageIndex = 1, int pageSize = 5)
        {
            var results = await _userService.GetAllUsersAsync(pageIndex, pageSize);
            return StatusCode(results.StatusCode, results);
        }



        [Route("users")]
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all users no pagings.",
                           Description = "This API returns all users that existed in system.")]
        [SwaggerResponse(200, "User retrieves successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetAllUsers()
        {
            var results = await _userService.GetAllUsersAsync();
            return StatusCode(results.StatusCode, results);
        }

        [Route("{roleName}/users")]
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all users with following role name.",
                           Description = "This API returns all users based on role name that existed in system.")]
        [SwaggerResponse(200, "User retrieves successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var results = await _userService.GetUsersByRoleAsync(roleName);
            return StatusCode(results.StatusCode, results);
        }

        [Route("{userId}/ban")]
        [HttpPut]
        [SwaggerOperation(Summary = "Ban user",
                           Description = "This API bans user based on user Id")]
        [SwaggerResponse(200, "User has been banned successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> BanUser(string userId)
        {
            var results = await _userService.BanUserAsync(userId);
            return StatusCode(results.StatusCode, results);
        }

        [Route("{userId}/unban")]
        [SwaggerOperation(Summary = "Un-ban user",
                           Description = "This API un-bans user based on user Id")]
        [SwaggerResponse(200, "User has been un-banned successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        [HttpPut]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            var results = await _userService.UnbanUserAsync(userId);
            return StatusCode(results.StatusCode, results);
        }

        [Route("{userId}/delete")]
        [HttpPut]
        [SwaggerOperation(Summary = "Delete user",
                           Description = "This API deletes user based on user Id")]
        [SwaggerResponse(200, "User has been deleted successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Route("requests")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all requests ",
                           Description = "This API gets all first-time login Google requests")]
        [SwaggerResponse(200, "All requests have been retrieved successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetAllRequests(int pageNumber = 1, int pageSize = 5)
        {
            var results = await _loginRequestService.GetAllRequests(pageNumber, pageSize);
            return StatusCode(results.StatusCode, results);
        }

        [Route("request")]
        [HttpPut]
        [SwaggerOperation(
    Summary = "Approve or reject requests",
    Description = "This API is used to approve or reject a user's first-time Google login request."
)]
        [SwaggerResponse(200, "Login request processed successfully.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> ApproveLoginRequest([FromQuery] Guid requestId, [FromQuery] ApprovalStatus status, [FromQuery] string? reason)
        {
            var statusString = status.GetDescription();
            var results = await _loginRequestService.ApprovalLoginGoogle(requestId, statusString, reason);
            return StatusCode(results.StatusCode, results);
        }
    }
}
