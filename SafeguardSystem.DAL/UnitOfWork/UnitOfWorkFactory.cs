using Microsoft.Extensions.DependencyInjection;
using SafeguardSystem.DAL.DBContext;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.DAL.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork Create()
        {
            var context = _serviceProvider.GetRequiredService<SafeguardDbContext>();
            return new UnitOfWork(context);
        }
    }
}
