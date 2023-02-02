using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public BasketRepository(WatchDbContext context)
        {
            _db = new UnitOfWorks.UnitOfWorks(context);
        }
        public async Task<Basket?> CreateAsync(Basket entity)
        {
            var details = entity.Details;

            var model = await _db.Baskets.CreateAsync(new BasketModel()
            {
                Date = entity.Date,
                UserId = entity.UserId
            });

            if (model == null)
            {
                return null;
            }

            entity = new Basket(model)
            {
                Details = details
            };

            entity.Details.ForEach(async detail =>
            {
                var watch = await _db.Watches.GetAsync(detail.WatchId);
                if (watch == null || watch.Available < detail.Count || watch.OnSale == false)
                {
                    return;
                }

                await _db.BasketDetails.CreateAsync(new BasketDetailModel()
                {
                    BasketId = entity.Id,
                    WatchId = detail.WatchId,
                    Count = detail.Count,
                    UnitPrice = detail.UnitPrice
                });


            });

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Baskets.DeleteAsync(id);
        }

        public async Task<IEnumerable<Basket>> GetAsync()
        {
            var models = await _db.Baskets.GetAsync();

            List<Basket> baskets = new List<Basket>();

            foreach (var model in models)
            {
                var details = (await _db.BasketDetails.GetByBasketIdAsync(model.Id)).Select(model => new BasketDetail(model)).ToList();
                baskets.Add(new Basket(model)
                {
                    Details = details
                });
            }

            return baskets;
        }

        public async Task<Basket?> GetAsync(int id)
        {
            var model = await _db.Baskets.GetAsync(id);

            if (model == null)
            {
                return null;
            }

            var details = (await _db.BasketDetails.GetByBasketIdAsync(model.Id)).Select(model => new BasketDetail(model)).ToList();

            return new Basket(model)
            {
                Details = details
            };
        }

        public async Task<Basket?> GetByUserIdAsync(string userId)
        {
            var model = await _db.Baskets.GetByUserIdAsync(userId);

            if (model == null)
            {
                return null;
            }

            var details = (await _db.BasketDetails.GetByBasketIdAsync(model.Id)).Select(model => new BasketDetail(model)).ToList();

            return new Basket(model)
            {
                Details = details
            };
        }

        public async Task<Basket> UpdateAsync(Basket entity)
        {
            foreach(var detail in entity.Details)
            {
                if(detail.Count == 0)
                {
                    await _db.BasketDetails.DeleteAsync(detail.Id);
                    continue;
                }

                var d = await _db.BasketDetails.GetAsync(detail.Id);

                if (d == null)
                {
                    d = await _db.BasketDetails.CreateAsync((BasketDetailModel)detail);
                }
                else
                {
                    d.Count = detail.Count;
                    await _db.BasketDetails.UpdateAsync(d);
                }
            }
            return entity;
        }
    }
}
