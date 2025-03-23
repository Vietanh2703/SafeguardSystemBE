using System.Linq.Expressions;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;

namespace SafeguardSystem.DAL.IRepositories;

public interface ILocationRepository
{
    Task<PaginatedList<Location>> GetAllLocations(int pageNumber, int pageSize);
    Task<List<Location>> GetAllLocations();
    Task<Location> GetLocationByName(string locationName);
    Task AddLocation(Location location);
    Task<Location> GetByCoordinatesAsync(decimal latitude, decimal longitude);
    Task<List<Location>> GetLocationByBusinessId(Guid businessId);
    Task<Location> GetLocationByIdAsync(Guid locationId);
    Task<Location> GetAsync(Expression<Func<Location, bool>> predicate);
}