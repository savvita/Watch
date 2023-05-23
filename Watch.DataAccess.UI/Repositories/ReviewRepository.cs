using Watch.DataAccess.UI.Exceptions;
using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public ReviewRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Review?> CreateAsync(Review entity)
        {
            entity.Text = entity.Text.Trim();
            var model = await _db.Reviews.CreateAsync((ReviewModel)entity);
            return model != null ? new Review(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Reviews.DeleteAsync(id);
        }

        public async Task<IEnumerable<Review>> GetAsync()
        {
            return (await _db.Reviews.GetAsync()).Select(model => new Review(model));
        }

        public async Task<Review?> GetAsync(int id)
        {
            var model = await _db.Reviews.GetAsync(id);

            return model != null ? new Review(model) : null;
        }

        public async Task<List<Review>> GetByUserIdAsync(string userId)
        {
            return (await _db.Reviews.GetByUserIdAsync(userId)).Select(model => new Review(model)).ToList();
        }

        public async Task<List<Review>> GetByWatchIdAsync(int watchId)
        {
            return (await _db.Reviews.GetByWatchIdAsync(watchId)).Select(model => new Review(model)).ToList();
        }

        public async Task<Review> UpdateAsync(Review entity)
        {
            entity.Text = entity.Text.Trim();
            var res = await _db.Reviews.UpdateConcurrencyAsync((ReviewModel)entity);
            //if(res.Code != 200)
            //{
            //    throw new ConcurrencyUpdateException();
            //}

            return entity;
        }
    }
}
