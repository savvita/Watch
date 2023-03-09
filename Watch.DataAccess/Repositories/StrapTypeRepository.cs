using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class StrapTypeRepository : GenericRepository<StrapTypeModel>, IStrapTypeRepository
    {
        public StrapTypeRepository(WatchDbContext db) : base(db)
        {

        }
    }
}
