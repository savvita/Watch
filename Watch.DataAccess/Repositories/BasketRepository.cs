using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class BasketRepository : GenericRepository<BasketModel>, IBasketRepository
    {
        public BasketRepository(WatchDbContext context) : base(context)
        {
        }
        public async Task<BasketModel?> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult<BasketModel?>(_db.Baskets.Where(b => b.UserId.Equals(userId)).FirstOrDefault());
        }
    }
}
