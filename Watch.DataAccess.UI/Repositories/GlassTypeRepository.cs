using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class GlassTypeRepository : IGlassTypeRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public GlassTypeRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<GlassType?> CreateAsync(GlassType entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.GlassTypes.CreateAsync((GlassTypeModel)entity);
            return model != null ? new GlassType(model) : null;

        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var models = await _db.GlassTypes.GetSalesAsync();
            var entities = new List<Sale>();

            foreach (var model in models)
            {
                if (model != null)
                {
                    entities.Add(new Sale(model));
                }
            }

            return entities;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.GlassTypeId == id)).ToList().ForEach(w => w.GlassTypeId = null);
            return await _db.GlassTypes.DeleteAsync(id);
        }

        public async Task<IEnumerable<GlassType>> GetAsync()
        {
            return (await _db.GlassTypes.GetAsync()).Select(model => new GlassType(model));
        }

        public async Task<GlassType?> GetAsync(int id)
        {
            var model = await _db.GlassTypes.GetAsync(id);

            return model != null ? new GlassType(model) : null;
        }

        public async Task<GlassType> UpdateAsync(GlassType entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.GlassTypes.UpdateAsync((GlassTypeModel)entity);

            return new GlassType(model);
        }
    }
}
