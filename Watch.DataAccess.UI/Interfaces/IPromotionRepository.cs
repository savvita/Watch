namespace Watch.DataAccess.UI.Interfaces
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Task<IEnumerable<Promotion>> GetAsync(bool activeOnly);
        Task<ConcurrencyUpdateResult> UpdateConcurrencyAsync(Promotion entity);
    }
}
