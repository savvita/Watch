using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class StyleRepository : IStyleRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public StyleRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Style?> CreateAsync(Style entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Styles.CreateAsync((StyleModel)entity);
            return model != null ? new Style(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.StyleId == id)).ToList().ForEach(w => w.StyleId = null);
            return await _db.Styles.DeleteAsync(id);
        }

        public async Task<IEnumerable<Style>> GetAsync()
        {
            return (await _db.Styles.GetAsync()).Select(model => new Style(model));
        }

        public async Task<Style?> GetAsync(int id)
        {
            var model = await _db.Styles.GetAsync(id);

            return model != null ? new Style(model) : null;
        }

        public async Task<Style> UpdateAsync(Style entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Styles.UpdateAsync((StyleModel)entity);

            return new Style(model);
        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var models = await _db.Styles.GetSalesAsync();
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
    }
}
