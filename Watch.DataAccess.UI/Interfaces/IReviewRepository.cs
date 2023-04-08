namespace Watch.DataAccess.UI.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<List<Review>> GetByWatchIdAsync(int wathcId);
        Task<List<Review>> GetByUserIdAsync(string userId);
    }
}
