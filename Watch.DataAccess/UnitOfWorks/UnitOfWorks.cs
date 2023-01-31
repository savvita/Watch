using Watch.DataAccess.Repositories;
using Watch.Domain.Interfaces;

namespace Watch.DataAccess.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly WatchDbContext _db;
        public ICategoryRepository Categories { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IOrderRepository Orders { get; }
        public IProducerRepository Producers { get; }
        public IUserRepository Users { get; }
        public IWatchRepository Watches { get; }
        public IOrderStatusRepository OrderStatuses { get; }

        public UnitOfWorks(WatchDbContext context)
        {
            _db = context;
            Categories = new CategoryRepository(context);
            OrderDetails = new OrderDetailRepository(context);
            Orders = new OrderRepository(context);
            Producers = new ProducerRepository(context);
            Users = new UserRepository(context);
            Watches = new WatchRepository(context);
            OrderStatuses = new OrderStatusRepository(context);
        }

        public async void Dispose()
        {
            await _db.SaveChangesAsync();
            _db.Dispose();
        }
    }
}
