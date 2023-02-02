using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess.UI.Repositories;

namespace Watch.DataAccess.UI
{
    public class DbContext
    {
        public ICategoryRepository Categories { get; }
        public IOrderRepository Orders { get; }
        public IBasketRepository Baskets { get; }
        public IProducerRepository Producers { get; }
        public IOrderStatusRepository OrderStatuses { get; }
        public IUserRepository Users { get; }
        public IWatchRepository Watches { get; }

        private readonly WatchDbContext _context;

        public DbContext(WatchDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            Orders = new OrderRepository(context);
            Baskets = new BasketRepository(context);
            Producers = new ProducerRepository(context);
            OrderStatuses = new OrderStatusRepository(context);
            Users = new UserRepository(context);
            Watches = new WatchRepository(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
