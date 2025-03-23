using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class CheckpointRepository : GenericRepository<Checkpoint>, ICheckpointRepository
{
    public CheckpointRepository(SafeguardDbContext context) : base(context)
    {
    }
    // Implement any additional methods specific to Checkpoint repository
}