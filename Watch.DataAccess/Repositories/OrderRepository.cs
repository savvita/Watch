using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(WatchDbContext context) : base(context)
        {
        }

        public async Task<bool> CloseOrderAsync(int id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if(order == null || order.StatusId == 3 || order.StatusId == 4)
            {
                return false;
            }

            order.StatusId = 3;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult<IEnumerable<OrderModel>>(_db.Orders.Where(o => o.UserId == userId));
        }
    }
}
