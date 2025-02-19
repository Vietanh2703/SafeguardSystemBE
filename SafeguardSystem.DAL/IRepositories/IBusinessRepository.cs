using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface IBusinessRepository : IGenericRepository<Business>
    {
        // Add any additional methods specific to Business repository
        List<Business> GetAllBusiness();
    }
}
