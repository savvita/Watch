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
            var models = await _db.Watches.GetAsync();

            foreach(var model in models)
            {
                model.Category = await _db.Categories.GetAsync(model.CategoryId);
                model.Producer = await _db.Producers.GetAsync(model.ProducerId);
            }


            return models.Select((model) => new Watch.DataAccess.UI.Models.Watch(model));
        }

       

        public async Task<Watch.DataAccess.UI.Models.Watch?> GetAsync(int id)
        {
            var model = await _db.Watches.GetAsync(id);

            if(model == null)
            {
                return null;
            }

            model.Category = await _db.Categories.GetAsync(model.CategoryId);
            model.Producer = await _db.Producers.GetAsync(model.ProducerId);

            return new Watch.DataAccess.UI.Models.Watch(model);
        }

        public async Task<IEnumerable<Models.Watch>> GetAsync(  string? model, 
                                                                List<int>? categoryIds = null, 
                                                                List<int>? producerIds = null, 
                                                                decimal? minPrice = null, 
                                                                decimal? maxPrice = null, 
                                                                bool? onSale = null,
                                                                bool? isPopular = null)
        {
            var models = await _db.Watches.GetAsync(model, categoryIds, producerIds, minPrice, maxPrice, onSale, isPopular);

            foreach (var m in models)
            {
                m.Category = await _db.Categories.GetAsync(m.CategoryId);
                m.Producer = await _db.Producers.GetAsync(m.ProducerId);
            }


            return models.Select((model) => new Watch.DataAccess.UI.Models.Watch(model));
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var model = await _db.Watches.GetAsync(id);

            if (model == null || model.OnSale == true || model.Available == 0)
            {
                return false;
            }

            model.OnSale = true;

            await _db.Watches.UpdateAsync(model);

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var model = await _db.Watches.GetAsync(id);

            if(model == null || model.OnSale == false)
            {
                return false;
            }

            model.OnSale = false;

            await _db.Watches.UpdateAsync(model);

            return true;
        }

        public async Task<Watch.DataAccess.UI.Models.Watch> UpdateAsync(Watch.DataAccess.UI.Models.Watch entity)
        {
            var model = await _db.Watches.UpdateAsync((WatchModel)entity);

            return new Watch.DataAccess.UI.Models.Watch(model);
        }
    }
}
