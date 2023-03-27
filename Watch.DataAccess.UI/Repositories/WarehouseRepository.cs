using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public WarehouseRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }


        public async Task<List<Warehouse>> GetBySettlementRefAsync(string settlementRef)
        {
            return (await _db.Warehouses.GetBySettlementRefAsync(settlementRef)).Select(model => new Warehouse(model)).ToList();
        }

        public async Task<DateTime?> GetLastUpdateAsync()
        {
            return await _db.Warehouses.GetLastUpdateAsync();
        }

        public Task<bool> UpdateAsync(string apiKey, string url)
        {
            return _db.Warehouses.UpdateAsync(apiKey, url);
        }
    }
}
