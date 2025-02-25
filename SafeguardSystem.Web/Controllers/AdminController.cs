using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.DAL.UnitOfWork;
using System.Threading.Tasks;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL;

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
    }
}
