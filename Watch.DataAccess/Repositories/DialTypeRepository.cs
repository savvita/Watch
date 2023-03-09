using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class DialTypeRepository : GenericRepository<DialTypeModel>, IDialTypeRepository
    {
        public DialTypeRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
