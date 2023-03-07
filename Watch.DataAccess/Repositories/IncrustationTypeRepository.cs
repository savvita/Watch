using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class IncrustationTypeRepository : GenericRepository<IncrustationTypeModel>, IIncrustationTypeRepository
    {
        public IncrustationTypeRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
