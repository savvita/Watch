using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class WatchRepository : GenericRepository<WatchModel>, IWatchRepository
    {
        public WatchRepository(WatchDbContext context) : base(context)
        {
        }
    }
}
