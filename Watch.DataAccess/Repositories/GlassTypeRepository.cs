using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class GlassTypeRepository : GenericRepository<GlassTypeModel>, IGlassTypeRepository
    {
        public GlassTypeRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
