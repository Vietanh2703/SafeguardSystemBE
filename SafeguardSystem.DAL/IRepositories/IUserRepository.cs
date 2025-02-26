using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByFirebaseUidAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
    }
}
