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
        private readonly IEmailService _emailService;

        public AdminController(IBusinessService businessService, IUserService userService, IEmailService emailService)
        {
            _businessService = businessService;
            _userService = userService;
            _emailService = emailService;
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
            if (results.IsSuccess)
            {
                var activationToken = results.Message;
                Console.WriteLine(activationToken);
                var activationLink = Url.Action(nameof(VerifyEmail), "Admin", new { ActivationToken = activationToken }, Request.Scheme);
                await _emailService.SendActivationEmailAsync(createUserDTO.Email, activationLink);
            }
            return StatusCode(results.StatusCode, results);
        }

        [Route("verify-email")]
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string ActivationToken)
        {
            var results = await _userService.VerifyEmailAsync(ActivationToken);
            return Redirect($"http://localhost:5173/");
        }
    }
}
