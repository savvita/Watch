namespace Watch.DataAccess.UI.Interfaces
{
    public interface IWatchRepository : IGenericRepository<Watch.DataAccess.UI.Models.Watch>
    {
        IWatchCounter Count { get; }
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<ConcurrencyUpdateResult> UpdateConcurrencyAsync(Models.Watch entity);

        Task<Result<IEnumerable<Watch.DataAccess.UI.Models.Watch>>> GetAsync(int page, int perPage, WatchFilter? filters = null);
    }
}
