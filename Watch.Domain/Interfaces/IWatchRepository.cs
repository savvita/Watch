using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IWatchRepository : IGenericRepository<WatchModel>
    {
        Task<IEnumerable<WatchModel>> GetAsync(int page, int perPage, string? model,
                                                               List<int>? categoryIds = null,
                                                               List<int>? producerIds = null,
                                                               decimal? minPrice = null,
                                                               decimal? maxPrice = null,
                                                               bool? onSale = null);
    }
}
