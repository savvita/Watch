using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    internal class OrderStatusRepository : GenericRepository<OrderStatusModel>, IOrderStatusRepository
    {
        public OrderStatusRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }
    }
}
