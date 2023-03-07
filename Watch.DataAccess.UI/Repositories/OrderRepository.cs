using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;

        public OrderRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Order?> CreateAsync(Basket basket)
        {
            var model = await _db.CreateOrderAsync((BasketModel)basket);
            return model != null ? new Order(model) : null;
        }
        public async Task<Order?> CreateAsync(Order entity)
        {
            var model = await _db.Orders.CreateAsync((OrderModel)entity);

            return model != null ? new Order(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Orders.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            var models = await _db.Orders.GetAsync();
            var statuses = await _db.OrderStatuses.GetAsync();

            List<Order> orders = new List<Order>();

            foreach (var model in models)
            {
                var details = (await _db.OrderDetails.GetByOrderIdAsync(model.Id)).Select(model => new OrderDetail(model)).ToList();
                var status = statuses.FirstOrDefault(s => s.Id == model.StatusId);
                orders.Add(new Order(model)
                {
                    Details = details,
                    Status = status != null ? new OrderStatus(status) : null
                });
            }

            return orders;
        }

        public async Task<Order?> GetAsync(int id)
        {
            var model = await _db.Orders.GetAsync(id);

            if(model == null)
            {
                return null;
            }

            var details = (await _db.OrderDetails.GetByOrderIdAsync(model.Id)).Select(model => new OrderDetail(model)).ToList();
            var status = (await _db.OrderStatuses.GetAsync()).FirstOrDefault(s => s.Id == model.StatusId);

            return new Order(model)
            {
                Details = details,
                Status = status != null ? new OrderStatus(status) : null
            };
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            var models = await _db.Orders.GetByUserIdAsync(userId);

            List<Order> orders = new List<Order>();

            foreach (var model in models)
            {
                var details = (await _db.OrderDetails.GetByOrderIdAsync(model.Id)).Select(model => new OrderDetail(model)).ToList();
                orders.Add(new Order(model)
                {
                    Details = details
                });
            }

            return orders;
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            var model = await _db.Orders.UpdateAsync((OrderModel)entity);

            return new Order(model);
        }

        public async Task<IEnumerable<Order>> GetByManagerIdAsync(string managerId)
        {
            var models = await _db.Orders.GetByManagerIdAsync(managerId);

            List<Order> orders = new List<Order>();

            foreach (var model in models)
            {
                var details = (await _db.OrderDetails.GetByOrderIdAsync(model.Id)).Select(model => new OrderDetail(model)).ToList();
                orders.Add(new Order(model)
                {
                    Details = details
                });
            }

            return orders;
        }

        public async Task<bool> SetOrderStatusAsync(int id, int statusId)
        {
            return await _db.Orders.SetOrderStatusAsync(id, statusId);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await _db.Orders.SetOrderStatusAsync(orderId, 4);
        }

        public async Task<bool> CloseOrderAsync(int id)
        {
            return await _db.Orders.SetOrderStatusAsync(id, 3);
        }
    }
}
