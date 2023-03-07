using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class MovementTypeRepository : GenericRepository<MovementTypeModel>, IMovementTypeRepository
    {
        public MovementTypeRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
