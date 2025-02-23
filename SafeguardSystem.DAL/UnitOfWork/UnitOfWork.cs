using SafeguardSystem.DAL.IRepositories;
using SafeguardSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace SafeguardSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SafeguardDbContext _context;

        public UnitOfWork(SafeguardDbContext context)
        {
            _context = context;
            Businesses = new BusinessRepository(_context);
            Checkpoints = new CheckpointRepository(_context);
            /*Contracts = new ContractRepository(_context);
            Locations = new LocationRepository(_context);
            Securityguards = new SecurityguardRepository(_context);
            Securityshifts = new SecurityshiftRepository(_context);
            Shiftassignments = new ShiftassignmentRepository(_context);
            Shiftincidents = new ShiftincidentRepository(_context);
            Shifttypes = new ShifttypeRepository(_context);
            Shifttyperoutines = new ShifttyperoutineRepository(_context);*/ //làm sau
            Roles = new RoleRepository(_context);
            RefreshTokens = new RefreshTokenRepository(_context);
            Users = new UserRepository(_context);
        }

        public IBusinessRepository Businesses { get; private set; }
        public ICheckpointRepository Checkpoints { get; private set; }
        /*public IContractRepository Contracts { get; private set; }
        public ILocationRepository Locations { get; private set; }
        public ISecurityguardRepository Securityguards { get; private set; }
        public ISecurityshiftRepository Securityshifts { get; private set; }
        public IShiftassignmentRepository Shiftassignments { get; private set; }
        public IShiftincidentRepository Shiftincidents { get; private set; }
        public IShifttypeRepository Shifttypes { get; private set; }
        public IShifttyperoutineRepository Shifttyperoutines { get; private set; } */ //làm sau
        public IRoleRepository Roles { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }
        public IUserRepository Users { get; private set; }

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
}
