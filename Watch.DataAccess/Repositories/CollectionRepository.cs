using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class CollectionRepository : GenericRepository<CollectionModel>, ICollectionRepository
    {
        public CollectionRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
