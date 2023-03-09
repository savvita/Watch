using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class CaseShapeRepository : ICaseShapeRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public CaseShapeRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<CaseShape?> CreateAsync(CaseShape entity)
        {
            var model = await _db.CaseShapes.CreateAsync((CaseShapeModel)entity);
            return model != null ? new CaseShape(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.CaseShapeId == id)).ToList().ForEach(w => w.CaseShapeId = null);
            return await _db.CaseShapes.DeleteAsync(id);
        }

        public async Task<IEnumerable<CaseShape>> GetAsync()
        {
            return (await _db.CaseShapes.GetAsync()).Select(model => new CaseShape(model));
        }

        public async Task<CaseShape?> GetAsync(int id)
        {
            var model = await _db.CaseShapes.GetAsync(id);

            return model != null ? new CaseShape(model) : null;
        }

        public async Task<CaseShape> UpdateAsync(CaseShape entity)
        {
            var model = await _db.CaseShapes.UpdateAsync((CaseShapeModel)entity);

            return new CaseShape(model);
        }
    }
}
