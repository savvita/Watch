using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public BasketRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Basket?> CreateAsync(Basket entity)
        {
            //TODO check this
            var model = new BasketModel()
            {
                UserId = entity.UserId
            };

            entity.Details.ForEach(d =>
            {
                model.Details.Add(new BasketDetailModel()
                {
                    Count = d.Count,
                    WatchId = d.WatchId
                });
            });

            var created = await _db.Baskets.CreateAsync(model);
            if(model == null)
            {
                return null;
            }

            entity.Details.ForEach(async detail =>
            {
                var watch = await _db.Watches.GetAsync(detail.WatchId);

                if(watch == null)
                {
                    return;
                }
                detail.UnitPrice = watch.Discount == null ? watch.Price : watch.Price - watch.Price * (decimal)watch.Discount / 100;
            });

            return entity;

            //TODO delete comments

            //var details = entity.Details;

            //var model = await _db.Baskets.CreateAsync(new BasketModel()
            //{
            //    UserId = entity.UserId
            //});

            //if (model == null)
            //{
            //    return null;
            //}

            //entity = new Basket(model)
            //{
            //    Details = details
            //};

            //entity.Details.ForEach(async detail =>
            //{
            //    var watch = await _db.Watches.GetAsync(detail.WatchId);
            //    if (watch == null || watch.Available < detail.Count || watch.OnSale == false)
            //    {
            //        return ;
            //    }

            //    await _db.BasketDetails.CreateAsync(new BasketDetailModel()
            //    {
            //        BasketId = entity.Id,
            //        WatchId = detail.WatchId,
            //        Count = detail.Count
            //    });


            //});

            //return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Baskets.DeleteAsync(id);
        }

        public async Task<IEnumerable<Basket>> GetAsync()
        {
            return await Task.FromResult<List<Basket>>(new List<Basket>());
            //TODO delete comments
            //var models = await _db.Baskets.GetAsync();

            //List<Basket> baskets = new List<Basket>();

            //foreach (var model in models)
            //{
            //    var details = (await _db.BasketDetails.GetByBasketIdAsync(model.Id)).Select(model => new BasketDetail(model)).ToList();
            //    baskets.Add(new Basket(model)
            //    {
            //        Details = details
            //    });
            //}

            //return baskets;
        }

        public async Task<Basket?> GetAsync(int id)
        {
            var model = await _db.Baskets.GetAsync(id);

            if (model == null)
            {
                return null;
            }

            var details = (await _db.BasketDetails.GetByBasketIdAsync(model.Id)).Select(model => new BasketDetail(model)).ToList();

            details.ForEach(async d =>
            {
                var watch = await _db.Watches.GetAsync(d.WatchId);
                if (watch == null)
                {
                    return;
                }

                d.UnitPrice = watch.Discount == null ? watch.Price : watch.Price - watch.Price * (decimal)watch.Discount / 100;
            });

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

            details.ForEach(async d =>
            {
                var watch = await _db.Watches.GetAsync(d.WatchId);
                if (watch == null)
                {
                    return;
                }

                d.UnitPrice = watch.Discount == null ? watch.Price : watch.Price - watch.Price * (decimal)watch.Discount / 100;
            });

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
                    await _db.BasketDetails.CreateAsync((BasketDetailModel)detail);
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
