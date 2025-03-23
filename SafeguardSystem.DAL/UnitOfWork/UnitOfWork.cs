using SafeguardSystem.DAL.IRepositories;
using SafeguardSystem.DAL.Repositories;

namespace SafeguardSystem.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SafeguardDbContext _context;

    public UnitOfWork(SafeguardDbContext context)
    {
        _context = context;
        Attendences = new AttendenceRepository(_context);
        Businesses = new BusinessRepository(_context);
        Checkpoints = new CheckpointRepository(_context);
        SecurityGuards = new SecurityGuardRepository(_context);
        Locations = new LocationRepository(_context);
        LoginRequests = new LoginRequestRepository(_context);
        /*Contracts = new ContractRepository(_context);
        */ //làm sau
        Teams = new TeamRepository(_context);
        TeamGuards = new TeamGuardRepository(_context);
        SecurityShifts = new SecurityShiftRepository(_context);
        ShiftTypes = new ShiftTypeRepository(_context);
        Roles = new RoleRepository(_context);
        RefreshTokens = new RefreshTokenRepository(_context);
        Reports = new ReportRepository(_context);
        Users = new UserRepository(_context);
    }

    public IAttendenceRepository Attendences { get; }
    public IBusinessRepository Businesses { get; }
    public ICheckpointRepository Checkpoints { get; }
    public ISecurityGuardRepository SecurityGuards { get; }
    public ILocationRepository Locations { get; }

    public ILoginRequestRepository LoginRequests { get; }

    /*public IContractRepository Contracts { get; private set; }*/ //làm sau
    public ITeamRepository Teams { get; }
    public ITeamGuardRepository TeamGuards { get; }
    public IRoleRepository Roles { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    public ISecurityShiftRepository SecurityShifts { get; }
    public IShiftTypeRepository ShiftTypes { get; }
    public IUserRepository Users { get; }
    public IReportRepository Reports { get; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public bool SaveChange()
    {
        return _context.SaveChanges() > 0;
    }


    public async Task<bool> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}