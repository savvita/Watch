namespace Watch.DataAccess.UI.Interfaces
{
    public interface ISlideRepository : IGenericRepository<Slide>
    {
        Task<IEnumerable<Slide>> GetAsync(bool activeOnly);
        Task<ConcurrencyUpdateResult> UpdateConcurrencyAsync(Slide entity);
    }
}
