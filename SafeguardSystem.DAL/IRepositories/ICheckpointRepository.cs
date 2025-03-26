using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface ICheckpointRepository : IGenericRepository<Checkpoint>
{
    Task<IEnumerable<Checkpoint>> GetCheckpointsByLocationIdAsync(Guid locationId);
    Task AddAsync(Checkpoint checkpoint);
    Task DeleteAsync(Guid id);
}