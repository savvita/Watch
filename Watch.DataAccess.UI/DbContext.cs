using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Repositories;

namespace Watch.DataAccess.UI
{
    public class DbContext
    {
        public ICategoryRepository Categories { get; }
        public IOrderRepository Orders { get; }
        public IProducerRepository Producers { get; }
        public IOrderStatusRepository OrderStatuses { get; }
        public IUserRepository Users { get; }
        public IWatchRepository Watches { get; }

        public DbContext(WatchDbContext context)
        {
            Categories = new CategoryRepository(context);
            Orders = new OrderRepository(context);
            Producers = new ProducerRepository(context);
            OrderStatuses = new OrderStatusRepository(context);
            Users = new UserRepository(context);
            Watches = new WatchRepository(context);
        }
    }
}
