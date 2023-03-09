using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class BasketDetailRepository : GenericRepository<BasketDetailModel>, IBasketDetailRepository
    {
        public BasketDetailRepository(WatchDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<BasketDetailModel>> GetByBasketIdAsync(int basketId)
        {
            return await Task.FromResult<IEnumerable<BasketDetailModel>>(_db.BasketDetails.Where(detail => detail.BasketId == basketId));
        }
    }
}
