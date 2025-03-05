using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.Common.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Controllers
{
    [Route("api/[controller]")]
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

        [Route("view-all-business-partners")]
        [HttpGet]
        public ResponseDTO GetAllBusinesses()
        {
            var results = _businessService.GetAllBusinessesAsync();
            return results;
        }

        [Route("create-user")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            var results = await _userService.CreateUserAsync(createUserDTO);
            return StatusCode(results.StatusCode, results);
        }

        [Route("view-all-users")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int pageIndex, int pageSize)
        {
            var results = await _userService.GetAllUsersAsync(pageIndex, pageSize);
            return StatusCode(results.StatusCode, results);
        }

        [Route("get-all-users")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var results = await _userService.GetAllUsersAsync();
            return StatusCode(results.StatusCode, results);
        }

        [Route("get-users-by-role/{roleName}")]
        [HttpGet]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var results = await _userService.GetUsersByRoleAsync(roleName);
            return StatusCode(results.StatusCode, results);
        }

        [Route("ban-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> BanUser(string userId)
        {
            var results = await _userService.BanUserAsync(userId);
            return StatusCode(results.StatusCode, results);
        }

        [Route("unban-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            var results = await _userService.UnbanUserAsync(userId);
            return StatusCode(results.StatusCode, results);
        }

        [Route("delete-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Route("get-all-requests")]
        [HttpGet]
        public async Task<IActionResult> GetAllRequests(int pageNumber, int pageSize)
        {
            var results = await _loginRequestService.GetAllRequests(pageNumber, pageSize);
            return StatusCode(results.StatusCode, results);
        }

        [Route("approve-login-request")]
        [HttpPut]
        [SwaggerOperation(Summary = "Approve or reject Google login user")]
        public async Task<IActionResult> ApproveLoginRequest([FromQuery] Guid requestId, [FromQuery] ApprovalStatus status, [FromQuery] string? reason)
        {
            var statusString = status.GetDescription();
            var results = await _loginRequestService.ApprovalLoginGoogle(requestId, statusString, reason);
            return StatusCode(results.StatusCode, results);
        }
    }
}
