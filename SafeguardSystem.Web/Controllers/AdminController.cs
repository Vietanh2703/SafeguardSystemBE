using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;

        public AdminController(IBusinessService businessService, IUserService userService)
        {
            _businessService = businessService;
            _userService = userService;
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

        [Route("delete-user/{userId}")]
        [HttpPut]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
