using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBusinessRepository Businesses { get; }
        ICheckpointRepository Checkpoints { get; }
        ILocationRepository Locations { get; }
        /*IContractRepository Contracts { get; }
        ISecurityguardRepository Securityguards { get; }
        ISecurityshiftRepository Securityshifts { get; }
        IShiftassignmentRepository Shiftassignments { get; }
        IShiftincidentRepository Shiftincidents { get; }
        IShifttypeRepository Shifttypes { get; }
        IShifttyperoutineRepository Shifttyperoutines { get; } */ //làm sau
        IRoleRepository Roles { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IUserRepository Users { get; }

        void Dispose();
        Task<bool> SaveChangeAsync();
        bool SaveChange();
    }
}
