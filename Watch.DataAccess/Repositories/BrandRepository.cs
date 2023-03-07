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

    }
}
