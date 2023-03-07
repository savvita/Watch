using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class CaseShapeRepository : GenericRepository<CaseShapeModel>, ICaseShapeRepository
    {
        public CaseShapeRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
