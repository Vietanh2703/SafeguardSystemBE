using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface ILocationRepository
    {
        Task<PaginatedList<Location>> GetAllLocations(int pageNumber, int pageSize);
        Task<Location> GetLocationByName(string locationName);
        Task AddLocation(Location location);
        Task<Location> GetByCoordinatesAsync(decimal latitude, decimal longitude);
        Task<List<Location>> GetLocationByBusinessId(Guid businessId);
        Task<Location> GetLocationByIdAsync(Guid locationId);
    }
}
