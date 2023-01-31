using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public CategoryRepository(WatchDbContext context)
        {
            _db = new UnitOfWorks.UnitOfWorks(context);
        }
        public async Task<Category?> CreateAsync(Category entity)
        {
            var model = await _db.Categories.CreateAsync((CategoryModel)entity);

            return model != null ? new Category(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Categories.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            return (await _db.Categories.GetAsync()).Select(model => new Category(model));
        }

        public async Task<Category?> GetAsync(int id)
        {
            var model = await _db.Categories.GetAsync(id);

            return model != null ? new Category(model) : null;
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            var model = await _db.Categories.UpdateAsync((CategoryModel)entity);

            return new Category(model);
        }
    }
}
