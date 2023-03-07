using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class BasketRepository : GenericRepository<BasketModel>, IBasketRepository
    {
        public BasketRepository(WatchDbContext db) : base(db)
        {
        }

        private bool ClearBasket(int id)
        {
            var model = _db.Baskets.FirstOrDefault(b => b.Id == id);
            if (model == null)
            {
                return false;
            }

            for (int i = 0; i < model.Details.Count(); i++)
            {
                _db.BasketDetails.Remove(model.Details.ElementAt(i));
            }

            return true;
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(ClearBasket(id));
        }

        public new async Task<BasketModel?> CreateAsync(BasketModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var newModel = await _db.Baskets.AddAsync(model);

                    //TODO Check adding details
                    model.Details.ToList().ForEach(async detail =>
                    {
                        var watch = _db.Watches.FirstOrDefault(w => w.Id == detail.WatchId);
                        if (watch == null || watch.Available < detail.Count || watch.OnSale == false)
                        {
                            transaction.Rollback();
                        }

                        await _db.BasketDetails.AddAsync(new BasketDetailModel()
                        {
                            BasketId = newModel.Entity.Id,
                            WatchId = detail.WatchId,
                            Count = detail.Count
                        });
                    });

                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return newModel.Entity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<BasketModel?> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult<BasketModel?>(_db.Baskets.Where(b => b.UserId.Equals(userId)).FirstOrDefault());
        }
    }
}
