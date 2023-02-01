using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(WatchDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult<IEnumerable<OrderModel>>(_db.Orders.Where(o => o.UserId == userId));
        }
    }
}
