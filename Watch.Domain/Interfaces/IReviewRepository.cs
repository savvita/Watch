using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<ReviewModel>
    {
        Task<List<ReviewModel>> GetByWatchIdAsync(int watchId);
    }
}
