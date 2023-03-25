using Microsoft.AspNetCore.Identity;
using Watch.DataAccess.Repositories;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly WatchDbContext _db;
        public IOrderDetailRepository OrderDetails { get; }
        public IOrderRepository Orders { get; }
        public IBasketDetailRepository BasketDetails { get; }
        public IBasketRepository Baskets { get; }
        public IUserRepository Users { get; }
        public IWatchRepository Watches { get; }
        public IOrderStatusRepository OrderStatuses { get; }

        public UserManager<UserModel> UserManager { get; }
        public RoleManager<IdentityRole> Roles { get; }

        public IBrandRepository Brands { get; }

        public IWaterResistanceRepository WaterResistance { get; }

        public IMovementTypeRepository MovementTypes { get; }

        public IMaterialRepository Materials { get; }

        public IIncrustationTypeRepository IncrustationTypes { get; }

        public IGlassTypeRepository GlassTypes { get; }

        public IGenderRepository Genders { get; }

        public IFunctionRepository Functions { get; }

        public IDialTypeRepository DialTypes { get; }

        public ICountryRepository Countries { get; }

        public IColorRepository Colors { get; }

        public ICollectionRepository Collections { get; }

        public ICaseShapeRepository CaseShapes { get; }

        public IStyleRepository Styles { get; }

        public IStrapTypeRepository StrapTypes { get; }
        public IImageRepository Images { get; }
        public IPaymentRepository Payments { get; }
        public IDeliveryRepository Deliveries { get; }
        public ICityRepository Cities { get; }
        public IWarehouseRepository Warehouses { get; }

        public UnitOfWorks(WatchDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = context;
            OrderDetails = new OrderDetailRepository(context);
            Orders = new OrderRepository(context);
            BasketDetails = new BasketDetailRepository(context);
            Baskets = new BasketRepository(context);
            Brands = new BrandRepository(context);
            Users = new UserRepository(context);
            Watches = new WatchRepository(context);
            OrderStatuses = new OrderStatusRepository(context);
            StrapTypes = new StrapTypeRepository(context);
            Styles = new StyleRepository(context);
            WaterResistance = new WaterResistanceRepository(context);
            MovementTypes = new MovementTypeRepository(context);
            Materials = new MaterialRepository(context);
            IncrustationTypes = new IncrustationTypeRepository(context);
            GlassTypes = new GlassTypeRepository(context);
            Genders = new GenderRepository(context);
            Functions = new FunctionRepository(context);
            DialTypes = new DialTypeRepository(context);
            Countries = new CountryRepository(context);
            Colors = new ColorRepository(context);
            Collections = new CollectionRepository(context);
            CaseShapes = new CaseShapeRepository(context);
            Images = new ImageRepository(context);
            Deliveries= new DeliveryRepository(context);
            Payments = new PaymentRepository(context);
            Cities = new CityRepository(context);
            Warehouses= new WarehouseRepository(context);
            UserManager = userManager;
            Roles = roleManager;

        }

        public async void Dispose()
        {
            await _db.SaveChangesAsync();
            _db.Dispose();
        }

        public async Task<OrderModel?> CreateOrderAsync(BasketModel basket, OrderAdditionalInfoModel info)
        {
            // TODO check this

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = await Orders.CreateAsync(new OrderModel()
                    {
                        Date = DateTime.Now,
                        StatusId = 1,
                        UserId = basket.UserId,
                        PaymentId = info.PaymentId,
                        DeliveryId = info.DeliveryId,
                        FullName = info.FullName,
                        PhoneNumber = info.PhoneNumber,
                        CityId = info.SettlementRef,
                        WarehouseId = info.WarehouseRef
                    });

                    if (order == null)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    foreach (var detail in basket.Details)
                    {
                        if (!(await AddDetailToOrderAsync(order, detail))) 
                        {
                            transaction.Rollback();
                            return null;
                        };
                    }

                    await Baskets.DeleteAsync(basket.Id);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return order;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
            //foreach (var detail in basket.Details)
            //{
            //    var watch = _db.Watches.FirstOrDefault(w => w.Id == detail.WatchId);
            //    if (watch == null || watch.Available < detail.Count || watch.OnSale == false)
            //    {
            //        return null;
            //    }
            //}

            //var order = await Orders.CreateAsync(new OrderModel()
            //{
            //    Date = DateTime.Now,
            //    StatusId = 1,
            //    UserId = basket.UserId
            //});

            //if(order == null)
            //{
            //    return null;
            //}

            //foreach (var detail in basket.Details)
            //{
            //    if (!(await AddDetailToOrderAsync(order, detail))) return null;

            //    // TODO Remove comments

            //    //var watch = _db.Watches.FirstOrDefault(w => w.Id == detail.WatchId);
            //    //if (watch == null || watch.Available < detail.Count)
            //    //{
            //    //    return null;
            //    //}

            //    //watch.Available -= detail.Count;

            //    //if(watch.Available <= 0)
            //    //{
            //    //    watch.OnSale = false;
            //    //}

            //    //await Watches.UpdateAsync(watch);

            //    //var d = await OrderDetails.CreateAsync(new OrderDetailModel()
            //    //{
            //    //    OrderId = order.Id,
            //    //    WatchId = detail.WatchId,
            //    //    UnitPrice = watch.Price,
            //    //    Count = detail.Count
            //    //});

            //    //if(d == null)
            //    //{
            //    //    return null;
            //    //}

            //    //order.Details.Add(d);

            //    //await BasketDetails.DeleteAsync(detail.Id);
            //}

            //await Baskets.DeleteAsync(basket.Id);

            //return order;
        }

        private async Task<bool> AddDetailToOrderAsync(OrderModel order, BasketDetailModel detail)
        {
            var watch = _db.Watches.FirstOrDefault(w => w.Id == detail.WatchId);
            if (watch == null || watch.Available < detail.Count || !watch.OnSale)
            {
                return false;
            }

            watch.Available -= detail.Count;

            if (watch.Available <= 0)
            {
                watch.OnSale = false;
            }

            var res = await Watches.UpdateAsync(watch);

            if (res.Available < 0) return false;

            var d = await OrderDetails.CreateAsync(new OrderDetailModel()
            {
                OrderId = order.Id,
                WatchId = detail.WatchId,
                UnitPrice = watch.Discount != null ? watch.Price - watch.Price * (decimal)watch.Discount / 100 :  watch.Price,
                Count = detail.Count
            });
            if (d == null)
            {
                return false;
            }

            order.Details.Add(d);

            await BasketDetails.DeleteAsync(detail.Id);

            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
            if(order == null || order.StatusId == 3 || order.StatusId == 4)
            {
                return false;
            }

            foreach(var detail in order.Details)
            {
                var watch = _db.Watches.FirstOrDefault(w => w.Id == detail.WatchId);
                if(watch == null)
                {
                    continue;
                }

                watch.Available += detail.Count;
            }

            await Orders.SetOrderStatusAsync(orderId, 4);

            //TODO Remove comments

            //order.StatusId = 4;

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
