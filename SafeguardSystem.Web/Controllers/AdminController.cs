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

        [Route("admin/view-all-business-partners")]
        [HttpGet]
        public ResponseDTO GetAllBusinesses()
        {
            var results = _businessService.GetAllBusinessesAsync();
            return results;
        }

        [Route("admin/create-user")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            var results = await _userService.CreateUserAsync(createUserDTO);
            return StatusCode(results.StatusCode, results);
        }

        [Route("admin/view-all-users")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int pageIndex, int pageSize)
        {
            var results = await _userService.GetAllUsersAsync(pageIndex, pageSize);
            return StatusCode(results.StatusCode, results);
        }

        [Route("admin/delete-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Route("admin/get-all-requests")]
        [HttpGet]
        public async Task<IActionResult> GetAllRequests(int pageNumber, int pageSize)
        {
            var results = await _loginRequestService.GetAllRequests(pageNumber, pageSize);
            return StatusCode(results.StatusCode, results);
        }

        [Route("admin/approve-login-request")]
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
