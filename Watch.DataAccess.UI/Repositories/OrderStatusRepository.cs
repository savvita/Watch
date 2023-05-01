using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public OrderStatusRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<OrderStatus?> CreateAsync(OrderStatus entity)
        {
            entity.Value = entity.Value.Trim();
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
            entity.Value = entity.Value.Trim();
            var model = await _db.OrderStatuses.UpdateAsync((OrderStatusModel)entity);

            return new OrderStatus(model);
        }
    }
}
