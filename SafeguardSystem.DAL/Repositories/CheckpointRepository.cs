using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class CheckpointRepository : GenericRepository<Checkpoint>, ICheckpointRepository
{
    private readonly SafeguardDbContext _context;

    public CheckpointRepository(SafeguardDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Checkpoint>> GetAllAsync()
    {
        return await _context.Checkpoints.ToListAsync();
    }
    

    public async Task<IEnumerable<Checkpoint>> GetCheckpointsByLocationIdAsync(Guid locationId)
    {
        return await _context.Checkpoints
            .Where(c => c.LocationId == locationId)
            .ToListAsync();
    }

    public async Task AddAsync(Checkpoint checkpoint)
    {
        await _context.Checkpoints.AddAsync(checkpoint);
        await _context.SaveChangesAsync();
    }
    

    public async Task DeleteAsync(Guid id)
    {
        var checkpoint = await _context.Checkpoints.FindAsync(id);
        if (checkpoint != null)
        {
            _context.Checkpoints.Remove(checkpoint);
            await _context.SaveChangesAsync();
        }
    } 
}