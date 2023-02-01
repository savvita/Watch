using Watch.DataAccess.Repositories;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly WatchDbContext _db;
        public ICategoryRepository Categories { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IOrderRepository Orders { get; }
        public IBasketDetailRepository BasketDetails { get; }
        public IBasketRepository Baskets { get; }
        public IProducerRepository Producers { get; }
        public IUserRepository Users { get; }
        public IWatchRepository Watches { get; }
        public IOrderStatusRepository OrderStatuses { get; }

        public UnitOfWorks(WatchDbContext context)
        {
            _db = context;
            Categories = new CategoryRepository(context);
            OrderDetails = new OrderDetailRepository(context);
            Orders = new OrderRepository(context);
            BasketDetails = new BasketDetailRepository(context);
            Baskets = new BasketRepository(context);
            Producers = new ProducerRepository(context);
            Users = new UserRepository(context);
            Watches = new WatchRepository(context);
            OrderStatuses = new OrderStatusRepository(context);
        }

        public async void Dispose()
        {
            await _db.SaveChangesAsync();
            _db.Dispose();
        }


        public async Task<OrderModel?> CreateOrderAsync(BasketModel basket)
        {
            foreach (var detail in basket.Details)
            {
                var watch = _db.Watches.FirstOrDefault(w => w.Id == basket.Id);
                if (watch == null || watch.Available < detail.Count || watch.OnSale == false)
                {
                    return null;
                }
            }

            var order = await Orders.CreateAsync(new OrderModel()
            {
                Date = DateTime.Now,
                StatusId = 1,
                UserId = basket.UserId
            });

            if(order == null)
            {
                return null;
            }

            foreach (var detail in basket.Details)
            {
                var watch = _db.Watches.FirstOrDefault(w => w.Id == basket.Id);
                if (watch == null || watch.Available < detail.Count)
                {
                    return null;
                }

                watch.Available -= detail.Count;
                watch.Sold += detail.Count;

                if(watch.Available <= 0)
                {
                    watch.OnSale = false;
                }

                await Watches.UpdateAsync(watch);

                var d = await OrderDetails.CreateAsync(new OrderDetailModel()
                {
                    OrderId = order.Id,
                    WatchId = detail.WatchId,
                    UnitPrice = detail.UnitPrice,
                    Count = detail.Count
                });

                if(d == null)
                {
                    return null;
                }

                order.Details.Add(d);

                await BasketDetails.DeleteAsync(detail.Id);
            }

            await Baskets.DeleteAsync(basket.Id);

            return order;
        }
    }
}
