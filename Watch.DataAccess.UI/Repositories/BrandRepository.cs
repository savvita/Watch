using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public BrandRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Brand?> CreateAsync(Brand entity)
        {
            var model = await _db.Brands.CreateAsync((BrandModel)entity);
            return model != null ? new Brand(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.BrandId == id)).ToList().ForEach(w => w.BrandId = null);
            return await _db.Brands.DeleteAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetAsync()
        {
            return (await _db.Brands.GetAsync()).Select(model => new Brand(model));
        }

        public async Task<Brand?> GetAsync(int id)
        {
            var model = await _db.Brands.GetAsync(id);

            return model != null ? new Brand(model) : null;
        }

        public async Task<Brand> UpdateAsync(Brand entity)
        {
            var model = await _db.Brands.UpdateAsync((BrandModel)entity);

            return new Brand(model);
        }
    }
}
