using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class BrandRepository : GenericRepository<BrandModel>, IBrandRepository
    {
        public BrandRepository(WatchDbContext db) : base(db)
        {
        }
        public new async Task<IEnumerable<BrandModel>> GetAsync()
        {
            return await Task.FromResult(_db.Brands.Include(model => model.Country));
        }

        public async Task<List<SaleModel>> GetSalesAsync()
        {
            var watches = _db.Watches.Where(w => w.BrandId != null).Select(w => w.Id);

            var res = await Task.FromResult(_db.OrderDetails
                .Where(detail => detail.Order != null && (detail.Order.StatusId == 3 || detail.Order.StatusId == 7))
                .Where(detail => watches.Contains(detail.WatchId))
                .Include(detail => detail.Watch)
                .ThenInclude(watch => watch!.Brand)
                .Select(item => new SaleModel
                {
                    Date = item.Order!.Date,
                    Watch = item.Watch!,
                    Count = item.Count,
                    UnitPrice = item.UnitPrice
                })
                .ToList());

            return res;
        }
    }
}
