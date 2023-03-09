using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class WaterResistanceRepository : GenericRepository<WaterResistanceModel>, IWaterResistanceRepository
    {
        public WaterResistanceRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
