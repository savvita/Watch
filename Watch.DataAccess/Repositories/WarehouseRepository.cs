using Watch.Domain.Interfaces;
using Watch.Domain.Models;
using Watch.NovaPoshta;

namespace Watch.DataAccess.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly WatchDbContext _db;
        public WarehouseRepository(WatchDbContext db)
        {
            _db = db;
        }

        public Task<List<WarehouseModel>> GetBySettlementRefAsync(string settlementRef)
        {
            return Task.FromResult(_db.Warehouses.Where(x => x.CityRef == settlementRef && x.WarehouseStatus.ToLower() == "working").ToList());
        }

        public async Task<DateTime?> GetLastUpdateAsync()
        {
            var warehouse = await Task.FromResult(_db.Warehouses.OrderByDescending(x => x.LastUpdate).FirstOrDefault());

            return warehouse == null ? null : warehouse.LastUpdate;
        }

        public async Task<bool> UpdateAsync(string apiKey, string url)
        {
            //var res = await UpdateAsync(apiKey, url, "841339c7-591a-42e2-8233-7a0a00f0ed6f");

            //if(!res)
            //{
            //    return false;
            //}

            return await UpdateAsync(apiKey, url, "9a68df70-0267-42a8-bb5c-37f427e36ee4");
            
        }

        private async Task<bool> UpdateAsync(string apiKey, string url, string type)
        {
            NovaPoshtaAddress np = new NovaPoshtaAddress(apiKey, url);
            try
            {
                var warehouses = await np.GetWarehousesAsync(type);
                warehouses.ForEach(async c =>
                {
                    var model = _db.Warehouses.FirstOrDefault(x => x.Ref == c.Ref);

                    if (model != null)
                    {
                        model.WarehouseStatus = c.WarehouseStatus;
                        model.Number = c.Number;
                        model.Description = c.Description;
                        model.SettlementDescription = c.SettlementDescription;
                        model.SettlementAreaDescription = c.SettlementAreaDescription;
                        model.CityRef = c.CityRef;
                        model.LastUpdate = DateTime.Now;
                        _db.Warehouses.Update(model);
                    }
                    else
                    {
                        await _db.Warehouses.AddAsync(new WarehouseModel(c));
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
