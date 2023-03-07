using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class IncrustationTypeRepository : IIncrustationTypeRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public IncrustationTypeRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<IncrustationType?> CreateAsync(IncrustationType entity)
        {
            var model = await _db.IncrustationTypes.CreateAsync((IncrustationTypeModel)entity);
            return model != null ? new IncrustationType(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.IncrustationTypeId == id)).ToList().ForEach(w => w.IncrustationTypeId = null);
            return await _db.IncrustationTypes.DeleteAsync(id);
        }

        public async Task<IEnumerable<IncrustationType>> GetAsync()
        {
            return (await _db.IncrustationTypes.GetAsync()).Select(model => new IncrustationType(model));
        }

        public async Task<IncrustationType?> GetAsync(int id)
        {
            var model = await _db.IncrustationTypes.GetAsync(id);

            return model != null ? new IncrustationType(model) : null;
        }

        public async Task<IncrustationType> UpdateAsync(IncrustationType entity)
        {
            var model = await _db.IncrustationTypes.UpdateAsync((IncrustationTypeModel)entity);

            return new IncrustationType(model);
        }
    }
}
