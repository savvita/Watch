using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public MaterialRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Material?> CreateAsync(Material entity)
        {
            var model = await _db.Materials.CreateAsync((MaterialModel)entity);
            return model != null ? new Material(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.CaseMaterialId == id)).ToList().ForEach(w => w.CaseMaterialId = null);
            return await _db.Materials.DeleteAsync(id);
        }

        public async Task<IEnumerable<Material>> GetAsync()
        {
            return (await _db.Materials.GetAsync()).Select(model => new Material(model));
        }

        public async Task<Material?> GetAsync(int id)
        {
            var model = await _db.Materials.GetAsync(id);

            return model != null ? new Material(model) : null;
        }

        public async Task<Material> UpdateAsync(Material entity)
        {
            var model = await _db.Materials.UpdateAsync((MaterialModel)entity);

            return new Material(model);
        }
    }
}
