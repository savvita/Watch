using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class DialTypeRepository : IDialTypeRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public DialTypeRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<DialType?> CreateAsync(DialType entity)
        {
            var model = await _db.DialTypes.CreateAsync((DialTypeModel)entity);
            return model != null ? new DialType(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.DialTypeId == id)).ToList().ForEach(w => w.DialTypeId = null);
            return await _db.DialTypes.DeleteAsync(id);
        }

        public async Task<IEnumerable<DialType>> GetAsync()
        {
            return (await _db.DialTypes.GetAsync()).Select(model => new DialType(model));
        }

        public async Task<DialType?> GetAsync(int id)
        {
            var model = await _db.DialTypes.GetAsync(id);

            return model != null ? new DialType(model) : null;
        }

        public async Task<DialType> UpdateAsync(DialType entity)
        {
            var model = await _db.DialTypes.UpdateAsync((DialTypeModel)entity);

            return new DialType(model);
        }
    }
}
