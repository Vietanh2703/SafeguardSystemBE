using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IAttendenceRepository Attendences { get; }
    IBusinessRepository Businesses { get; }
    ICheckpointRepository Checkpoints { get; }
    ILocationRepository Locations { get; }
    ISecurityGuardRepository SecurityGuards { get; }

    ILoginRequestRepository LoginRequests { get; }

    /*IContractRepository Contracts { get; }
    ISecurityshiftRepository Securityshifts { get; }
    IShiftincidentRepository Shiftincidents { get; }
    IShifttyperoutineRepository Shifttyperoutines { get; } */ //làm sau
    ITeamRepository Teams { get; }
    ITeamGuardRepository TeamGuards { get; }
    IRoleRepository Roles { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    ISecurityShiftRepository SecurityShifts { get; }
    IShiftTypeRepository ShiftTypes { get; }
    IReportRepository Reports { get; }
    IUserRepository Users { get; }

    void Dispose();
    Task<bool> SaveChangeAsync();
    bool SaveChange();
}