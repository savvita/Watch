using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Watch.Domain.Models;

namespace Watch.DataAccess
{
    public class WatchDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<WatchModel> Watches { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<CaseShapeModel> CaseShapes { get; set; }
        public DbSet<CollectionModel> Collections { get; set; }
        public DbSet<ColorModel> Colors { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<DialTypeModel> DialTypes { get; set; }
        public DbSet<FunctionModel> Functions { get; set; }
        public DbSet<GenderModel> Genders { get; set; }
        public DbSet<GlassTypeModel> GlassTypes { get; set; }
        public DbSet<IncrustationTypeModel> IncrustationTypes { get; set; }
        public DbSet<MaterialModel> Materials { get; set; }
        public DbSet<MovementTypeModel> MovementTypes { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<BasketDetailModel> BasketDetails { get; set; }
        public DbSet<BasketModel> Baskets { get; set; }
        public DbSet<OrderStatusModel> OrderStatuses { get; set; }
        public DbSet<StrapTypeModel> StrapTypes { get; set; }
        public DbSet<StyleModel> Styles { get; set; }
        public DbSet<WaterResistanceModel> WaterResistance { get; set; }

        public DbSet<ImageModel> Images { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<DeliveryModel> Deliveries { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<WarehouseModel> Warehouses { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }

        public WatchDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderStatusModel>().HasData(
                new OrderStatusModel { Id = 1, Value = "Новий" },
                new OrderStatusModel { Id = 2, Value = "В обробці" },
                new OrderStatusModel { Id = 3, Value = "Виконано" },
                new OrderStatusModel { Id = 4, Value = "Скасовано" },
                new OrderStatusModel { Id = 5, Value = "Оплачено" },
                new OrderStatusModel { Id = 6, Value = "Очікує оплати" },
                new OrderStatusModel { Id = 7, Value = "Відправлено" }
            );
            modelBuilder.Entity<GenderModel>().HasData(
                new GenderModel { Id = 1, Value = "Чоловіча" },
                new GenderModel { Id = 2, Value = "Жіноча" },
                new GenderModel { Id = 3, Value = "Унісекс" }
            );
            modelBuilder.Entity<PaymentModel>().HasData(
                new PaymentModel { Id = 1, Value = "Передоплата на розрахунковий рахунок", IsActive = true },
                new PaymentModel { Id = 2, Value = "Післяплата", IsActive = true }
            );
            modelBuilder.Entity<DeliveryModel>().HasData(
                new DeliveryModel { Id = 1, Value = "Самовивіз", IsActive = true },
                new DeliveryModel { Id = 2, Value = "Нова Пошта", IsActive = true }
            );
        }
    }
}
