using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class CountryRepository : GenericRepository<CountryModel>, ICountryRepository
    {
        public CountryRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
