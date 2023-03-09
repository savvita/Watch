using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }

        public Task<IEnumerable<OrderModel>> GetByManagerIdAsync(string managerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult<IEnumerable<OrderModel>>(_db.Orders.Where(o => o.UserId == userId));
        }

        public async Task<bool> SetOrderStatusAsync(int id, int statusId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null || order.StatusId == 3 || order.StatusId == 4)
            {
                return false;
            }

            order.StatusId = statusId;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }

        // TODO delete this
        public async Task<bool> CloseOrderAsync(int id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null || order.StatusId == 3 || order.StatusId == 4)
            {
                return false;
            }

            order.StatusId = 3;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
