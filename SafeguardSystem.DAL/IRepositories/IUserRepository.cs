using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByFirebaseUidAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);


    }
}
