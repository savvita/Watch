using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public CollectionRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Collection?> CreateAsync(Collection entity)
        {
            var model = await _db.Collections.CreateAsync((CollectionModel)entity);
            return model != null ? new Collection(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.CollectionId == id)).ToList().ForEach(w => w.CollectionId = null);
            return await _db.Collections.DeleteAsync(id);
        }

        public async Task<IEnumerable<Collection>> GetAsync()
        {
            return (await _db.Collections.GetAsync()).Select(model => new Collection(model));
        }

        public async Task<Collection?> GetAsync(int id)
        {
            var model = await _db.Collections.GetAsync(id);

            return model != null ? new Collection(model) : null;
        }

        public async Task<Collection> UpdateAsync(Collection entity)
        {
            var model = await _db.Collections.UpdateAsync((CollectionModel)entity);

            return new Collection(model);
        }
    }
}
