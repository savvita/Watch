using Microsoft.AspNetCore.Identity;
using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Repositories;

namespace Watch.DataAccess.UI
{
    public class DbContext
    {
        private readonly WatchDbContext _context;
        public IBrandRepository Brands { get; }
        public IOrderRepository Orders { get; }
        public IBasketRepository Baskets { get; }
        public ICaseShapeRepository CaseShapes { get; }
        public ICollectionRepository Collections { get; }
        public IColorRepository Colors { get; }
        public ICountryRepository Countries { get; }
        public IDialTypeRepository DialTypes { get; }
        public IFunctionRepository Functions { get; }
        public IGenderRepository Genders { get; }
        public IGlassTypeRepository GlassTypes { get; }
        public IIncrustationTypeRepository IncrustationTypes { get; }
        public IMaterialRepository Materials { get; }
        public IMovementTypeRepository MovementTypes { get; }
        public IOrderStatusRepository OrderStatuses { get; }
        public IStrapTypeRepository StrapTypes { get; }
        public IStyleRepository Styles { get; }
        public IUserRepository Users { get; }
        public IWatchRepository Watches { get; }
        public IWaterResistanceRepository WaterResistance { get; }
        public IImageRepository Images { get; }
        public IPaymentRepository Payments { get; }
        public IDeliveryRepository Deliveries { get; }
        public ICityRepository Cities { get; }
        public IWarehouseRepository Warehouses { get; }
        public IReviewRepository Reviews { get; }
        public ISlideRepository Slides { get; }
        public IPromotionRepository Promotions { get; }


        public DbContext(WatchDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            var uow = new UnitOfWorks.UnitOfWorks(context, userManager, roleManager);

            Baskets = new BasketRepository(uow);
            Brands = new BrandRepository(uow);
            CaseShapes = new CaseShapeRepository(uow);
            Collections = new CollectionRepository(uow);
            Colors = new ColorRepository(uow);
            Countries = new CountryRepository(uow);
            DialTypes = new DialTypeRepository(uow);
            Functions = new FunctionRepository(uow);
            Genders = new GenderRepository(uow);
            GlassTypes = new GlassTypeRepository(uow);
            IncrustationTypes = new IncrustationTypeRepository(uow);
            Materials = new MaterialRepository(uow);
            MovementTypes = new MovementTypeRepository(uow);
            Orders = new OrderRepository(uow);
            OrderStatuses = new OrderStatusRepository(uow);
            StrapTypes = new StrapTypeRepository(uow);
            Styles = new StyleRepository(uow);
            Users = new UserRepository(uow);
            Watches = new WatchRepository(uow);
            WaterResistance = new WaterResistanceRepository(uow);
            Images = new ImageRepository(uow);
            Payments = new PaymentRepository(uow);
            Deliveries = new DeliveryRepository(uow);
            Cities = new CityRepository(uow);
            Warehouses = new WarehouseRepository(uow);
            Reviews = new ReviewRepository(uow);
            Slides = new SlideRepository(uow);
            Promotions = new PromotionRepository(uow);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
