using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class GenderRepository : GenericRepository<GenderModel>, IGenderRepository
    {
        public GenderRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
