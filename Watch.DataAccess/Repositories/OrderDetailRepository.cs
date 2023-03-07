using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetailModel>, IOrderDetailRepository
    {
        public OrderDetailRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }
        public async Task<IEnumerable<OrderDetailModel>> GetByOrderIdAsync(int orderId)
        {
            return await Task.FromResult<IEnumerable<OrderDetailModel>>(_db.OrderDetails.Where(detail => detail.OrderId == orderId));
        }
    }
}
