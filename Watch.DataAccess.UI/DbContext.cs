using Microsoft.AspNetCore.Identity;
using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Repositories;
using Watch.Domain.Models;

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

        public DbContext(WatchDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            Categories = new CategoryRepository(context, userManager, roleManager);
            Orders = new OrderRepository(context, userManager, roleManager);
            Baskets = new BasketRepository(context, userManager, roleManager);
            Producers = new ProducerRepository(context, userManager, roleManager);
            OrderStatuses = new OrderStatusRepository(context, userManager, roleManager);
            Users = new UserRepository(context, userManager, roleManager);
            Watches = new WatchRepository(context, userManager, roleManager);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
