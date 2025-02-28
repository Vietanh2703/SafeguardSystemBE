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
        //Task<Location> GetLocationById(Guid locationId);
        //Task<Location> GetLocationByName(string locationName);
        //Task AddLocation(Location location);
        //Task UpdateLocation(Location location);
        //Task DeleteLocation(Guid locationId);

    }
}
