using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
    public class CheckpointRepository : GenericRepository<Checkpoint>, ICheckpointRepository
    {
        public CheckpointRepository(SafeguardDbContext context) : base(context)
        {
        }
        // Implement any additional methods specific to Checkpoint repository
    }

}

