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
    public class RefreshTokenRepository : GenericRepository<RefreshToken>,IRefreshTokenRepository
    {
        private readonly SafeguardDbContext _context;

        public RefreshTokenRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetRefreshTokenByUserID(Guid userId)
        {
            
            return await _context.Refreshtokens
                                 .Where(x => x.UserId == userId && !x.IsRevoked)
                                 .FirstOrDefaultAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenByKey(string refreshTokenKey)
        {
            if(string.IsNullOrEmpty(refreshTokenKey))
            {
                throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshTokenKey));
            }

            //Tìm requestToken theo requestTokenKey
            var requestTokenKey = await _context.Refreshtokens
                                                .FirstOrDefaultAsync(x => x.RefreshTokenKey == refreshTokenKey);

            return requestTokenKey;
        }
    }
}
