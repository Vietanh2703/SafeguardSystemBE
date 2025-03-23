using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    private readonly SafeguardDbContext _context;

    public LocationRepository(SafeguardDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Location>> GetAllLocations(int pageNumber, int pageSize)
    {
        var query = _context.Locations.AsQueryable();
        return await PaginatedList<Location>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<List<Location>> GetAllLocations()
    {
        return await _context.Locations.ToListAsync();
    }

    // Lấy Location theo tên
    public async Task<Location> GetLocationByName(string locationName)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(loc => loc.Name == locationName);
    }


    // Thêm Location mới
    public async Task AddLocation(Location location)
    {
        await _context.Locations.AddAsync(location);
    }

    // Lấy Location theo tọa độ
    public async Task<Location> GetByCoordinatesAsync(decimal latitude, decimal longitude)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(loc => loc.Latitude == latitude && loc.Longitude == longitude);
    }

    // Lấy Location theo BusinessId
    public async Task<List<Location>> GetLocationByBusinessId(Guid businessId)
    {
        return await _context.Locations
            .Where(loc => loc.BusinessId == businessId)
            .ToListAsync();
    }

    public async Task<Location> GetLocationByIdAsync(Guid locationId)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(loc => loc.LocationId == locationId);
    }
    
    public async Task<Location> GetAsync(Expression<Func<Location, bool>> predicate)
    {
        return await _context.Locations.FirstOrDefaultAsync(predicate);
    }
}