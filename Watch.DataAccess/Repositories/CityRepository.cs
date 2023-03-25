using Watch.Domain.Interfaces;
using Watch.Domain.Models;
using Watch.NovaPoshta;

namespace Watch.DataAccess.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly WatchDbContext _db;
        public CityRepository(WatchDbContext db)
        {
            _db = db;
        }

        public async Task<List<CityModel>> GetAsync()
        {
            return await Task.FromResult(_db.Cities.ToList());
        }

        public async Task<DateTime?> GetLastUpdateAsync()
        {
            var city = await Task.FromResult(_db.Cities.OrderByDescending(x => x.LastUpdate).FirstOrDefault());

            return city == null ? null : city.LastUpdate;
        }

        //TODO check this
        public async Task<bool> UpdateAsync(string apiKey, string url)
        {
            NovaPoshtaAddress np = new NovaPoshtaAddress(apiKey, url);
            try
            {
                var cities = await np.GetCitiesAsync();
                cities.ForEach(async c =>
                {
                    var model = _db.Cities.FirstOrDefault(x => x.Ref == c.Ref);

                    if (model != null)
                    {
                        model.AreaDescription = c.AreaDescription;
                        model.Description = c.Description;
                        model.LastUpdate = DateTime.Now;
                        _db.Cities.Update(model);
                    }
                    else
                    {
                        await _db.Cities.AddAsync(new CityModel(c));
                    }
                });

                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
