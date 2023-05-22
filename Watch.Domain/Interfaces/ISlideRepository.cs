
using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface ISlideRepository : IGenericRepository<SlideModel>
    {
        Task<IEnumerable<SlideModel>> GetAsync(bool activeOnly);
        Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(SlideModel entity);
    }
}
