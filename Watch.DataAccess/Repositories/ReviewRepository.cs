using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class ReviewRepository : GenericRepository<ReviewModel>, IReviewRepository
    {
        public ReviewRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            var model = await _db.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            if(model == null || model.Deleted == true)
            {
                return false;
            }

            model.Deleted = true;
            model.Checked = true;

            _db.Update(model);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<ReviewModel>> GetByWatchIdAsync(int watchId)
        {
            var models = await Task.FromResult(_db.Reviews.Where(x => x.WatchId == watchId).ToList());

            models.ForEach(model =>
            {
                if (model.Deleted)
                {
                    model.Text = "";
                }
            });

            return models;
        }
    }
}
