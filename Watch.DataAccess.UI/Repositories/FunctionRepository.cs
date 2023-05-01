using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class FunctionRepository : IFunctionRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public FunctionRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Function?> CreateAsync(Function entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Functions.CreateAsync((FunctionModel)entity);
            return model != null ? new Function(model) : null;

        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var models = await _db.Functions.GetSalesAsync();
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
            return await _db.Functions.DeleteAsync(id);
        }

        public async Task<IEnumerable<Function>> GetAsync()
        {
            return (await _db.Functions.GetAsync()).Select(model => new Function(model));
        }

        public async Task<Function?> GetAsync(int id)
        {
            var model = await _db.Functions.GetAsync(id);

            return model != null ? new Function(model) : null;
        }

        public async Task<Function> UpdateAsync(Function entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Functions.UpdateAsync((FunctionModel)entity);

            return new Function(model);
        }
    }
}
