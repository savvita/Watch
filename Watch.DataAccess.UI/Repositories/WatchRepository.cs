using Watch.DataAccess.UI.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class WatchRepository : IWatchRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public WatchRepository(WatchDbContext context)
        {
            _db = new UnitOfWorks.UnitOfWorks(context);
        }
        public async Task<Watch.DataAccess.UI.Models.Watch?> CreateAsync(Watch.DataAccess.UI.Models.Watch entity)
        {
            var model = await _db.Watches.CreateAsync((WatchModel)entity);

            return model != null ? new Watch.DataAccess.UI.Models.Watch(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Watches.DeleteAsync(id);
        }

        public async Task<IEnumerable<Watch.DataAccess.UI.Models.Watch>> GetAsync()
        {
            return (await _db.Watches.GetAsync()).Select(model => new Watch.DataAccess.UI.Models.Watch(model));
        }

        public async Task<Watch.DataAccess.UI.Models.Watch?> GetAsync(int id)
        {
            var model = await _db.Watches.GetAsync(id);

            return model != null ? new Watch.DataAccess.UI.Models.Watch(model) : null;
        }

        public async Task<Watch.DataAccess.UI.Models.Watch> UpdateAsync(Watch.DataAccess.UI.Models.Watch entity)
        {
            var model = await _db.Watches.UpdateAsync((WatchModel)entity);

            return new Watch.DataAccess.UI.Models.Watch(model);
        }
    }
}
