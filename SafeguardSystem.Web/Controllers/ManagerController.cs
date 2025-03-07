using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;

namespace SafeguardSystem.Web.Controllers
{
    public class ManagerController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ManagerController(IReportService reportService)
        {
            _reportService = reportService;
        }
        
        
    }
}
