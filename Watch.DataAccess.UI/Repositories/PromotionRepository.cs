
using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public PromotionRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Promotion?> CreateAsync(Promotion entity)
        {
            if(entity.DiscountValue < 0 || entity.DiscountValue > 100)
            {
                return null;
            }

            entity.Title = entity.Title.Trim();
            if(entity.Title.Length == 0)
            {
                return null;
            }

            if (entity.Description != null)
            {
                entity.Description = entity.Description.Trim();
            }
            
            var model = await _db.Promotions.CreateAsync((PromotionModel)entity);
            return model != null ? new Promotion(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _db.Promotions.GetAsync(id);
            if (model == null)
            {
                return false;
            }

            model.IsActive = false;

            await _db.Promotions.UpdateAsync(model);

            return true;
        }

        public async Task<IEnumerable<Promotion>> GetAsync()
        {
            return (await _db.Promotions.GetAsync()).Select(model => new Promotion(model));
        }

        public async Task<IEnumerable<Promotion>> GetAsync(bool activeOnly)
        {
            return (await _db.Promotions.GetAsync(activeOnly)).Select(model => new Promotion(model));
        }

        public async Task<Promotion?> GetAsync(int id)
        {
            var model = await _db.Promotions.GetAsync(id);

            return model != null ? new Promotion(model) : null;
        }

        public async Task<Promotion> UpdateAsync(Promotion entity)
        {
            if (entity.Description != null)
            {
                entity.Description = entity.Description.Trim();
            }

            var model = await _db.Promotions.UpdateAsync((PromotionModel)entity);

            return new Promotion(model);
        }

        public async Task<ConcurrencyUpdateResult> UpdateConcurrencyAsync(Promotion entity)
        {
            var model = await _db.Promotions.UpdateConcurrencyAsync((PromotionModel)entity);

            return new Models.ConcurrencyUpdateResult()
            {
                Code = model.Code,
                Message = model.Message
            };
        }
    }
}
