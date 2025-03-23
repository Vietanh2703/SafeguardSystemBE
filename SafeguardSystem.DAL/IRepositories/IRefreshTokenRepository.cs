using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken> GetRefreshTokenByUserID(string userId);

    Task<RefreshToken> GetRefreshTokenByKey(string refreshTokenKey);
}