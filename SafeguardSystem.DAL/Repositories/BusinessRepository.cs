using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
    public class BusinessRepository : GenericRepository<Business>, IBusinessRepository
    {
        private readonly SafeguardDbContext _context;
        public BusinessRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Business> GetAllBusiness()
        {
            return _context.Businesses.ToList();
        }

        public async Task<Business?> GetBusinessByUserIdAsync(string userId)
        {
            return await _context.Set<Business>()
                                 .FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task<Business> CreateAsync(Business business)
        {
            await _context.Businesses.AddAsync(business);
            await _context.SaveChangesAsync();
            return business;
        }
    }
}
