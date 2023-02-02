using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Watch.Domain.Models;

namespace Watch.DataAccess
{
    public class WatchDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<WatchModel> Watches { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProducerModel> Producers { get; set; }
        public DbSet<OrderDetailModel> OrderDetails{ get; set; }
        public DbSet<OrderModel> Orders{ get; set; }
        public DbSet<BasketDetailModel> BasketDetails { get; set; }
        public DbSet<BasketModel> Baskets { get; set; }
        public DbSet<OrderStatusModel> OrderStatuses{ get; set; }

        public WatchDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
