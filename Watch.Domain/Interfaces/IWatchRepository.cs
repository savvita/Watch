using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IWatchRepository : IGenericRepository<WatchModel>
    {
        Task<IEnumerable<WatchModel>> GetAsync( string? model,
                                                List<int>? categoryIds = null,
                                                List<int>? producerIds = null,
                                                decimal? minPrice = null,
                                                decimal? maxPrice = null,
                                                bool? onSale = null,
                                                bool? isPopular = null);
    }
}
