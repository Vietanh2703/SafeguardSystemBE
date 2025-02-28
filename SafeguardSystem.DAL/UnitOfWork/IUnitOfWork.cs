using SafeguardSystem.DAL.IRepositories;
using SafeguardSystem.DAL.Repositories;

namespace SafeguardSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBusinessRepository Businesses { get; }
        ICheckpointRepository Checkpoints { get; }
        ILocationRepository Locations { get; }
        ISecurityGuardRepository SecurityGuards { get; }
        /*IContractRepository Contracts { get; }
        ISecurityshiftRepository Securityshifts { get; }
        IShiftassignmentRepository Shiftassignments { get; }
        IShiftincidentRepository Shiftincidents { get; }
        IShifttypeRepository Shifttypes { get; }
        IShifttyperoutineRepository Shifttyperoutines { get; } */ //làm sau
        IRoleRepository Roles { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IShiftTypeRepository ShiftTypes { get; }
        IUserRepository Users { get; }

        void Dispose();
        Task<bool> SaveChangeAsync();
        bool SaveChange();
    }
}
