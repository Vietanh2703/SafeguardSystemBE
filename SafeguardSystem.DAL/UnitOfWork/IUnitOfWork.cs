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
        /*IContractRepository Contracts { get; }
        ILocationRepository Locations { get; }
        ISecurityguardRepository Securityguards { get; }
        ISecurityshiftRepository Securityshifts { get; }
        IShiftassignmentRepository Shiftassignments { get; }
        IShiftincidentRepository Shiftincidents { get; }
        IShifttypeRepository Shifttypes { get; }
        IShifttyperoutineRepository Shifttyperoutines { get; } */ //làm sau
        void Dispose();
        Task<bool> SaveChangeAsync();
        bool SaveChange();
    }
}
