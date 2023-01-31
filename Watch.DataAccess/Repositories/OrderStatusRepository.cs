using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    internal class OrderStatusRepository : GenericRepository<OrderStatusModel>, IOrderStatusRepository
    {
        public OrderStatusRepository(WatchDbContext context) : base(context)
        {
        }
    }
}
