using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class WaterResistanceRepository : IWaterResistanceRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public WaterResistanceRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<WaterResistance?> CreateAsync(WaterResistance entity)
        {
            var model = await _db.WaterResistance.CreateAsync((WaterResistanceModel)entity);
            return model != null ? new WaterResistance(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.WaterResistanceId == id)).ToList().ForEach(w => w.WaterResistanceId = null);
            return await _db.WaterResistance.DeleteAsync(id);
        }

        public async Task<IEnumerable<WaterResistance>> GetAsync()
        {
            return (await _db.WaterResistance.GetAsync()).Select(model => new WaterResistance(model));
        }

        public async Task<WaterResistance?> GetAsync(int id)
        {
            var model = await _db.WaterResistance.GetAsync(id);

            return model != null ? new WaterResistance(model) : null;
        }

        public async Task<WaterResistance> UpdateAsync(WaterResistance entity)
        {
            var model = await _db.WaterResistance.UpdateAsync((WaterResistanceModel)entity);

            return new WaterResistance(model);
        }
    }
}
