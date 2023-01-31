using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public OrderRepository(WatchDbContext context)
        {
            _db = new UnitOfWorks.UnitOfWorks(context);
        }
        public async Task<Order?> CreateAsync(Order entity)
        {
            var details = entity.Details;

            var model = await _db.Orders.CreateAsync(new OrderModel()
            {
                Date = entity.Date,
                StatusId = entity.Status != null ? entity.Status.Id : 0,
                UserId = entity.UserId
            });

            if(model == null)
            {
                return null;
            }

            entity = new Order(model)
            {
                Details = details
            };

            entity.Details.ForEach(async detail => 
            {
                var watch = await _db.Watches.GetAsync(detail.WatchId);
                if(watch == null || watch.Available < detail.Count || watch.OnSale == false)
                {
                    return;
                }

                watch.Available -= detail.Count;

                if(watch.Available == 0)
                {
                    watch.OnSale = false;
                }

                watch.Sold += detail.Count;

                await _db.Watches.UpdateAsync(watch);

                await _db.OrderDetails.CreateAsync(new OrderDetailModel()
                {
                    OrderId = entity.Id,
                    WatchId = detail.WatchId,
                    Count = detail.Count,
                    UnitPrice = detail.UnitPrice
                });


            });

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.OrderDetails.GetAsync())
                .Where(detail => detail.OrderId == id)
                .ToList()
                .ForEach(async (detail) => await _db.OrderDetails.DeleteAsync(detail.Id));

            return await _db.Orders.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            var models = await _db.Orders.GetAsync();

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

        public async Task<Order?> GetAsync(int id)
        {
            var model = await _db.Orders.GetAsync(id);

            if(model == null)
            {
                return null;
            }

            var details = (await _db.OrderDetails.GetByOrderIdAsync(model.Id)).Select(model => new OrderDetail(model)).ToList();

            return new Order(model)
            {
                Details = details
            };
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            entity.Details.ForEach(async (detail) => await _db.OrderDetails.UpdateAsync((OrderDetailModel)detail));

            var model = await _db.Orders.UpdateAsync((OrderModel)entity);

            return new Order(model);
        }
    }
}
