using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(WatchDbContext context) : base(context)
        {
        }
    }
}
