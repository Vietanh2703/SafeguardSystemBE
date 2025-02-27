using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByFirebaseUidAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<PaginatedList<User>> GetAllUsersWithPagingAsync(int pageIndex, int pageSize);
    }
}
