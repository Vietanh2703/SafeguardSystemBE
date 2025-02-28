using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
    public class LocationRepository: GenericRepository<Location>, ILocationRepository
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
    }
}
