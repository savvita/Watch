using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IWatchRepository : IGenericRepository<WatchModel>
    {
        IWatchCounter Count { get; }
        Task<ResultModel<List<WatchModel>>> GetAsync(int page, int perPage, WatchFilterModel? filters);
        Task<IEnumerable<WatchModel>> Where(Func<WatchModel, bool> predicate);
        Task<ConcurrencyUpdateResultModel<WatchModel>> UpdateConcurrencyAsync(WatchModel entity);
    }
}
