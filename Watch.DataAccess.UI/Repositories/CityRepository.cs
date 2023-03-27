using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public CityRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<List<City>> GetAsync()
        {
            return (await _db.Cities.GetAsync()).Select(model => new City(model)).ToList();
        }

        public async Task<DateTime?> GetLastUpdateAsync()
        {
            return await _db.Cities.GetLastUpdateAsync();
        }

        public Task<bool> UpdateAsync(string apiKey, string url)
        {
            return _db.Cities.UpdateAsync(apiKey, url);
        }
    }
}
