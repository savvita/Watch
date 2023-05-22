using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IPromotionRepository : IGenericRepository<PromotionModel>
    {
        Task<IEnumerable<PromotionModel>> GetAsync(bool activeOnly);
        Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(PromotionModel entity);
        Task<IEnumerable<PromotionModel>> Where(Func<PromotionModel, bool> predicate);
    }
}
