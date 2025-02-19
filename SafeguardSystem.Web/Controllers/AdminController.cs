using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.DAL.UnitOfWork;
using System.Threading.Tasks;
using SafeguardSystem.DAL.DBContext;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IBusinessService businessService, SafeguardDbContext safeguardDbContext)
        {
            _businessService = businessService;
        }

        [Route("view-all-business-partners")]
        [HttpGet]
        public ResponseDTO GetAllBusinesses()
        {
            var results = _businessService.GetAllBusinessesAsync();
            return results;
        }
    }
}
