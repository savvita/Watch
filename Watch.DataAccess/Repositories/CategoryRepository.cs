using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class CategoryRepository : GenericRepository<CategoryModel>, ICategoryRepository
    {
        public CategoryRepository(WatchDbContext context) : base(context)
        {
        }
    }
}
