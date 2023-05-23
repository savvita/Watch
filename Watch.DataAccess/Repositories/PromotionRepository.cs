using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class PromotionRepository : GenericRepository<PromotionModel>, IPromotionRepository
    {
        public PromotionRepository(WatchDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<PromotionModel>> GetAsync(bool activeOnly)
        {
            return await Task.FromResult(_db.Promotions.Where(x => x.IsActive == activeOnly).Include(x => x.Brand));
        }
        public new async Task<IEnumerable<PromotionModel>> GetAsync()
        {
            return await Task.FromResult(_db.Promotions.Include(x => x.Brand));
        }

        public new async Task<PromotionModel?> GetAsync(int id)
        {
            return await _db.Promotions.Where(x => x.Id == id).Include(x => x.Brand).FirstOrDefaultAsync();
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            var model = await _db.Promotions.FirstOrDefaultAsync(item => item.Id == id);
            if(model == null)
            {
                return false;
            }

            await _db.Slides.Where(item => item.PromotionId == id).ForEachAsync(item => item.IsActive = false);
            _db.Promotions.Remove(model);
            await _db.SaveChangesAsync();

            return true;
        }

        public new async Task<PromotionModel?> CreateAsync(PromotionModel entity)
        {
            if (entity.DiscountValue < 0 || entity.DiscountValue > 100)
            {
                return null;
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    entity = (await _db.Promotions.AddAsync(entity)).Entity;
                    if(entity.IsActive)
                    {
                        await AddDiscount(entity);
                    }
                    await _db.SaveChangesAsync();
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

        private bool CheckRowVersion(byte[] db, byte[] updated)
        {
            if(db.Length != updated.Length)
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

        public async Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(PromotionModel entity)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var model = (await _db.Promotions.FirstAsync(x => x.Id == entity.Id));
 

                    if(entity.IsActive && model.BrandId != null && model.BrandId != entity.BrandId)
                    {
                        await ChangeBrand(model, entity);
                    }


                    if (model.IsActive && !entity.IsActive)
                    {
                        if (model.BrandId != entity.BrandId)
                        {
                            await RemoveDiscount(model);
                            await RemoveDiscount(entity);
                        }
                        else
                        {
                            await RemoveDiscount(entity);
                        }
                    }
                    if (!model.IsActive && entity.IsActive)
                    {
                        await AddDiscount(entity);
                    }

                    model.StartDate = entity.StartDate;
                    model.EndDate = entity.EndDate;
                    model.Title = entity.Title.Trim();
                    model.Description = entity.Description;
                    model.DiscountValue = entity.DiscountValue;
                    model.IsActive = entity.IsActive;

                    if (entity.BrandId != null)
                    {
                        model.Brand = await _db.Brands.FirstOrDefaultAsync(x => x.Id == entity.BrandId);
                    }
                    else if(model.BrandId != null)
                    {
                        model.BrandId = null;
                    }

                    if (!CheckRowVersion(model.RowVersion, entity.RowVersion))
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    _db.Promotions.Update(model);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 200,
                        Message = "Ok"
                    };
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
                    var model = await _db.Promotions.FindAsync(entity.Id);

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
                catch(Exception)
                {
                    transaction.Rollback();
                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 500,
                        Message = "Internal Server Error"
                    };
                }
            }
        }

        private async Task AddDiscount(PromotionModel entity)
        {
            if(!entity.IsActive)
            {
                return;
            }

            if(entity.BrandId == null)
            {
                await _db.Promotions.Where(x => x.Id != entity.Id && x.IsActive).ForEachAsync(x => x.IsActive = false);
                await _db.Slides.Where(x => x.PromotionId != null && x.PromotionId != entity.Id).ForEachAsync(x => x.IsActive = false);
                await _db.Watches.ForEachAsync(x => x.Discount = entity.DiscountValue);
            }
            else
            {
                await _db.Promotions.Where(x => x.Id != entity.Id && x.IsActive && x.BrandId == entity.BrandId).ForEachAsync(x => x.IsActive = false);
                await _db.Slides
                    .Where(x => x.PromotionId != null)
                    .Include(x => x.Promotion)
                    .ThenInclude(x => x.Brand)
                    .Where(x => x.Promotion.Brand != null && x.Promotion.Id != entity.Id && x.Promotion.Brand.Id == entity.BrandId)
                    .ForEachAsync(x => x.IsActive = false);
                await _db.Watches.Where(x => x.BrandId == entity.BrandId).ForEachAsync(x => x.Discount = entity.DiscountValue);
            }
        }

        private async Task RemoveDiscount(PromotionModel entity)
        {
            if (entity.BrandId == null)
            {
                await _db.Watches.ForEachAsync(x => x.Discount = null);
            }
            else
            {
                await _db.Watches.Where(x => x.BrandId == entity.BrandId).ForEachAsync(x => x.Discount = null);
            }

            await _db.Slides.Where(x => x.PromotionId == entity.Id).ForEachAsync(x => x.IsActive = false);
        }

        private async Task ChangeBrand(PromotionModel oldModel, PromotionModel newModel)
        {
            if(!newModel.IsActive || !oldModel.IsActive)
            {
                return;
            }

            if(oldModel.BrandId == newModel.BrandId)
            {
                return;
            }

            if(newModel.BrandId == null)
            {
                await _db.Watches.ForEachAsync(x => x.Discount = newModel.DiscountValue);
            }
            else
            {
                if(oldModel.BrandId != null)
                {
                    await _db.Watches.Where(x => x.BrandId == oldModel.BrandId).ForEachAsync(x => x.Discount = null);
                    await _db.Watches.Where(x => x.BrandId == newModel.BrandId).ForEachAsync(x => x.Discount = newModel.DiscountValue);
                }
                else
                {
                    await _db.Watches.Where(x => x.BrandId != newModel.BrandId).ForEachAsync(x => x.Discount = null);
                }
            }
        }

        public async Task<IEnumerable<PromotionModel>> Where(Func<PromotionModel, bool> predicate)
        {
            return await Task.FromResult<IEnumerable<PromotionModel>>(_db.Promotions.Where(predicate));
        }
    }
}
