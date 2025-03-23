using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly SafeguardDbContext _context;

    public ReportRepository(SafeguardDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Report>> GetAllReportsAsync()
    {
        return await _context.Reports
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetReportsByUserRoleAsync(string role)
    {
        return await _context.Reports
            .Include(r => r.User)
            .Where(r => r.User.Role.RoleName == role)
            .ToListAsync();
    }

    public async Task<Report> GetReportByIdAsync(Guid reportId)
    {
        return await _context.Reports.FindAsync(reportId);
    }

    public async Task AddReportAsync(Report report)
    {
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();
    }
}