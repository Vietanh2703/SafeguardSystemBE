using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface IReportRepository : IGenericRepository<Report>
{
    Task<List<Report>> GetAllReportsAsync();
    Task<IEnumerable<Report>> GetReportsByUserRoleAsync(string role);
    Task<Report> GetReportByIdAsync(Guid reportId);
    Task AddReportAsync(Report report);
}