using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class MaterialRepository : GenericRepository<MaterialModel>, IMaterialRepository
    {
        public MaterialRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
