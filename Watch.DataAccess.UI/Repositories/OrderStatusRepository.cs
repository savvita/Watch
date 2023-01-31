using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public OrderStatusRepository(WatchDbContext context)
        {
            _db = new UnitOfWorks.UnitOfWorks(context);
        }
        public async Task<OrderStatus?> CreateAsync(OrderStatus entity)
        {
            var model = await _db.OrderStatuses.CreateAsync((OrderStatusModel)entity);

            return model != null ? new OrderStatus(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.OrderStatuses.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetAsync()
        {
            return (await _db.OrderStatuses.GetAsync()).Select(model => new OrderStatus(model));
        }

        public async Task<OrderStatus?> GetAsync(int id)
        {
            var model = await _db.OrderStatuses.GetAsync(id);

            return model != null ? new OrderStatus(model) : null;
        }

        public async Task<OrderStatus> UpdateAsync(OrderStatus entity)
        {
            var model = await _db.OrderStatuses.UpdateAsync((OrderStatusModel)entity);

            return new OrderStatus(model);
        }
    }
}
