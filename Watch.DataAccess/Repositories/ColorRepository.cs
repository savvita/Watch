using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class ColorRepository : GenericRepository<ColorModel>, IColorRepository
    {
        public ColorRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
