namespace Watch.DataAccess.UI.Interfaces
{
    public interface IWatchRepository : IGenericRepository<Watch.DataAccess.UI.Models.Watch>
    {
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);

        Task<IEnumerable<Watch.DataAccess.UI.Models.Watch>> GetAsync(int page, int perPage, string? model,
                                                               List<int>? categoryIds = null,
                                                               List<int>? producerIds = null,
                                                               decimal? minPrice = null,
                                                               decimal? maxPrice = null,
                                                               bool? onSale = null);
    }
}
