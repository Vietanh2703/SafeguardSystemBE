using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface IBusinessRepository : IGenericRepository<Business>
{
    // Add any additional methods specific to Business repository
    List<Business> GetAllBusiness();
    Task<Business?> GetBusinessByUserIdAsync(string userId);
    Task<Business> CreateAsync(Business business);
}