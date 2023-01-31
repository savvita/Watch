using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class WatchRepository : GenericRepository<WatchModel>, IWatchRepository
    {
        public WatchRepository(WatchDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WatchModel>> GetAsync(int page, int perPage, string? model,
                                                               List<int>? categoryIds = null,
                                                               List<int>? producerIds = null,
                                                               decimal? minPrice = null,
                                                               decimal? maxPrice = null,
                                                               bool? onSale = null)
        {
            var watches = _db.Watches.Include(w => w.Producer).ToList();

            if(model != null)
            {
                model = model.ToLower();
                watches = watches
                    .Where(w => w.Model.ToLower().Contains(model) || w.Producer != null && w.Producer.ProducerName.ToLower().Contains(model))
                    .ToList();
            }

            if(categoryIds != null)
            {
                watches = watches.Where(w => categoryIds.Contains(w.CategoryId)).ToList();
            }

            if (producerIds != null)
            {
                watches = watches.Where(w => producerIds.Contains(w.ProducerId)).ToList();
            }

            if (minPrice != null)
            {
                watches = watches.Where(w => w.Price >= minPrice).ToList();
            }

            if (maxPrice != null)
            {
                watches = watches.Where(w => w.Price <= maxPrice).ToList();
            }

            if (onSale != null)
            {
                watches = watches.Where(w => w.OnSale == onSale).ToList();
            }


            watches = watches.Skip((page - 1) * perPage).Take(perPage).ToList();

            return watches;
        }
    }
}
