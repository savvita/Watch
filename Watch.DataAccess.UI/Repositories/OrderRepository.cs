using Microsoft.AspNetCore.Identity;
using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public OrderRepository(WatchDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = new UnitOfWorks.UnitOfWorks(context, userManager, roleManager);
        }

        public Task<bool> CloseOrderAsync(int id)
        {
            return _db.Orders.CloseOrderAsync(id);
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

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await _db.CancelOrderAsync(orderId);
        }
    }
}
