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

        public new async Task<ReviewModel?> CreateAsync(ReviewModel entity)
        {
            if(entity.Rate != null && (entity.Rate < 0 || entity.Rate > 5))
            {
                return null;
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    entity = (await _db.Reviews.AddAsync(entity)).Entity;
                    await _db.SaveChangesAsync();

                    if (entity.Rate != null)
                    {
                        await UpdateRate(entity);
                        await _db.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return entity;
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        private async Task UpdateRate(ReviewModel entity)
        {
            var watch = await _db.Watches.FirstAsync(x => x.Id == entity.WatchId);
            var reviews = _db.Reviews.Where(x => x.WatchId == entity.WatchId && x.Rate != null && x.Deleted == false && x.Checked == true).ToList();
            //if (entity.Checked == true && entity.Deleted == false && entity.Rate != null)
            //{
            //    reviews.Add(entity);
            //}
            watch.Votes = reviews.Count();
            if (watch.Votes > 0)
            {
                watch.Rate = (reviews.Sum(x => x.Rate) ?? 0) / (float)reviews.Count();
            }
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

            if((await UpdateConcurrencyAsync(model)).Code != 200)
            {
                return false;
            }
            await UpdateRate(model);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<ReviewModel>> GetByUserIdAsync(string userId)
        {
            var models = await Task.FromResult(_db.Reviews.Where(x => x.UserId == userId).ToList());

            models.ForEach(model =>
            {
                if (model.Deleted)
                {
                    model.Text = "";
                }
            });

            return models;
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

        private bool CheckRowVersion(byte[] db, byte[] updated)
        {
            if (db.Length != updated.Length)
            {
                return false;
            }
            int length = db.Length;
            for (int i = 0; i < length; i++)
            {
                if (db[i] != updated[i])
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(ReviewModel entity)
        {
            if (entity.Rate != null && (entity.Rate < 0 || entity.Rate > 5))
            {
                return new ConcurrencyUpdateResultModel()
                {
                    Code = 409,
                    Message = "Validation error"
                };
            }

            try
            {
                var model = (await _db.Reviews.FirstAsync(x => x.Id == entity.Id));
                model.UserId = entity.UserId;
                model.UserName = entity.UserName;
                model.ReplyToId = entity.ReplyToId;
                model.Text = entity.Text;
                model.Checked = entity.Checked;
                model.Deleted = entity.Deleted;
                model.Date = entity.Date;
                model.WatchId = entity.WatchId;
                model.Rate = entity.Rate;

                if (!CheckRowVersion(model.RowVersion, entity.RowVersion))
                {
                    throw new DbUpdateConcurrencyException();
                }

                _db.Reviews.Update(model);
                await _db.SaveChangesAsync();
                await UpdateRate(model);
                await _db.SaveChangesAsync();

                return new ConcurrencyUpdateResultModel()
                {
                    Code = 200,
                    Message = "Ok"
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                var model = await _db.Watches.FindAsync(entity.Id);

                if (model == null)
                {
                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 404,
                        Message = "The entry was deleted"
                    };
                }

                return new ConcurrencyUpdateResultModel()
                {
                    Code = 409,
                    Message = "The entry was modified by another user"
                };
            }
        }

    }
}
