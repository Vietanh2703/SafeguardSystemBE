using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<List<Report>> GetAllReportsAsync();
        Task<IEnumerable<Report>> GetReportsByUserRoleAsync(string role);
        Task<Report> GetReportByIdAsync(Guid reportId);
        Task AddReportAsync(Report report);
    }
}
