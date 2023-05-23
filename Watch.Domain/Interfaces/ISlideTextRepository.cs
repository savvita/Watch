using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface ISlideTextRepository : IGenericRepository<TextModel>
    {
        Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(TextModel entity);
    }
}
