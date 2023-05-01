using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public ColorRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Color?> CreateAsync(Color entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Colors.CreateAsync((ColorModel)entity);
            return model != null ? new Color(model) : null;

        }

        public async Task<List<Sale>> GetSalesAsync(string type)
        {
            var models = await _db.Colors.GetSalesAsync(type);
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
            (await _db.Watches.Where(w => w.CaseColorId == id)).ToList().ForEach(w => w.CaseColorId = null);
            (await _db.Watches.Where(w => w.DialColorId == id)).ToList().ForEach(w => w.DialColorId = null);
            (await _db.Watches.Where(w => w.StrapColorId == id)).ToList().ForEach(w => w.StrapColorId = null);
            return await _db.Colors.DeleteAsync(id);
        }

        public async Task<IEnumerable<Color>> GetAsync()
        {
            return (await _db.Colors.GetAsync()).Select(model => new Color(model));
        }

        public async Task<Color?> GetAsync(int id)
        {
            var model = await _db.Colors.GetAsync(id);

            return model != null ? new Color(model) : null;
        }

        public async Task<Color> UpdateAsync(Color entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Colors.UpdateAsync((ColorModel)entity);

            return new Color(model);
        }
    }
}
