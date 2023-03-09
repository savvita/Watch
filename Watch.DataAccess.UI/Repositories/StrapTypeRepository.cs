using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class StrapTypeRepository : IStrapTypeRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public StrapTypeRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<StrapType?> CreateAsync(StrapType entity)
        {
            var model = await _db.StrapTypes.CreateAsync((StrapTypeModel)entity);
            return model != null ? new StrapType(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.StrapTypeId == id)).ToList().ForEach(w => w.StrapTypeId = null);
            return await _db.StrapTypes.DeleteAsync(id);
        }

        public async Task<IEnumerable<StrapType>> GetAsync()
        {
            return (await _db.StrapTypes.GetAsync()).Select(model => new StrapType(model));
        }

        public async Task<StrapType?> GetAsync(int id)
        {
            var model = await _db.StrapTypes.GetAsync(id);

            return model != null ? new StrapType(model) : null;
        }

        public async Task<StrapType> UpdateAsync(StrapType entity)
        {
            var model = await _db.StrapTypes.UpdateAsync((StrapTypeModel)entity);

            return new StrapType(model);
        }
    }
}
