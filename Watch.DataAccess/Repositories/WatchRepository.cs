using Microsoft.EntityFrameworkCore;
using System.Linq;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class WatchRepository : GenericRepository<WatchModel>, IWatchRepository
    {
        public IWatchCounter Count { get; }
        public WatchRepository(WatchDbContext db) : base(db)
        {
            Count = new WatchCounter(db);
        }
        public new async Task<WatchModel?> GetAsync(int id)
        {
            var entities = await Task.FromResult(_db.Watches.Where(w => w.Id == id));

            if(entities == null || entities.Count() == 0)
            {
                return null;
            }

            var e = entities
                .Include(w => w.Brand)
                .Include(w => w.Collection)
                .Include(w => w.Style)
                .Include(w => w.MovementType)
                .Include(w => w.GlassType)
                .Include(w => w.CaseShape)
                .Include(w => w.CaseMaterial)
                .Include(w => w.StrapType)
                .Include(w => w.CaseColor)
                .Include(w => w.StrapColor)
                .Include(w => w.DialColor)
                .Include(w => w.WaterResistance)
                .Include(w => w.IncrustationType)
                .Include(w => w.DialType)
                .Include(w => w.Gender)
                .Include(w => w.Images)
                .Include(w => w.Functions)
                .Include(w => w.Reviews)
                .ToList();

            return e.ElementAt(0);
        }

        public new async Task<WatchModel?> CreateAsync(WatchModel entity)
        {
            var functions = new List<FunctionModel>(entity.Functions);
            entity.Functions.Clear();

            foreach (var f in functions)
            {
                var item = await _db.Functions.FindAsync(f.Id);
                if(item != null)
                {
                    entity.Functions.Add(item);
                }
            }

            var images = new List<ImageModel>(entity.Images);
            entity.Images.Clear();

            foreach (var i in images)
            {
                var item = (await _db.Images.AddAsync(i)).Entity;
                if (item != null)
                {
                    entity.Images.Add(item);
                }
            }

            entity = (await _db.Watches.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();
            return entity;
        }

        //TODO Important! Check this. Check functions updating

        public async Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(WatchModel entity)
        {
            try
            {
                var model = (await _db.Watches.Include(x => x.Functions).FirstAsync(x => x.Id == entity.Id));
                model.Available = entity.Available;
                model.BrandId = entity.BrandId;
                model.CaseColorId = entity.CaseColorId;
                model.CaseMaterialId = entity.CaseMaterialId;
                model.CaseSize = entity.CaseShapeId;
                model.CaseSize = entity.CaseSize;
                model.CollectionId = entity.CollectionId;
                model.Description = entity.Description;
                model.DialColorId = entity.DialColorId;
                model.DialTypeId = entity.DialTypeId;
                model.Discount = entity.Discount;
                model.GenderId = entity.GenderId;
                model.GlassTypeId = entity.GlassTypeId;
                model.IncrustationTypeId = entity.IncrustationTypeId;
                model.IsTop = entity.IsTop;
                model.Model = entity.Model;
                model.MovementTypeId = entity.MovementTypeId;
                model.OnSale = entity.OnSale;
                model.Price = entity.Price;
                model.StrapColorId = entity.StrapColorId;
                model.StrapTypeId = entity.StrapTypeId;
                model.StyleId = entity.StyleId;
                model.Title = entity.Title;
                model.WaterResistanceId = entity.WaterResistanceId;
                model.Weight = entity.Weight;

                var selectedFunctions = entity.Functions.Select(x => x.Id).ToList();
                var functions = model.Functions.Select(x => x.Id).ToList();

                foreach (var f in _db.Functions)
                {
                    if(selectedFunctions.Contains(f.Id))
                    {
                        if (!functions.Contains(f.Id))
                        {
                            model.Functions.Add(f);
                        }
                        else
                        {
                            if(functions.Contains(f.Id))
                            {
                                model.Functions.Remove(f);
                            }
                        }
                    }
                }

                _db.Watches.Update(model);
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

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }

        private ResultModel<List<WatchModel>> GetWatches(int page, int perPage, WatchFilterModel? filters = null)
        {
            IQueryable<WatchModel> watches = _db.Watches;

            if (filters != null) {
                watches = _db.Watches
                .Where(w => (w.Model.Contains(filters.Model) || filters.Model.Length == 0) &&
                            (filters.BrandId.Contains(w.BrandId) || filters.BrandId.Count == 0) &&
                            (filters.CollectionId.Contains(w.CollectionId) || filters.CollectionId.Count == 0) &&
                            (filters.StyleId.Contains(w.StyleId) || filters.StyleId.Count == 0) &&
                            (filters.MovementTypeId.Contains(w.MovementTypeId) || filters.MovementTypeId.Count == 0) &&
                            (filters.GlassTypeId.Contains(w.GlassTypeId) || filters.GlassTypeId.Count == 0) &&
                            (filters.CaseShapeId.Contains(w.CaseShapeId) || filters.CaseShapeId.Count == 0) &&
                            (filters.CaseMaterialId.Contains(w.CaseMaterialId) || filters.CaseMaterialId.Count == 0) &&
                            (filters.StrapTypeId.Contains(w.StrapTypeId) || filters.StrapTypeId.Count == 0) &&
                            (filters.CaseColorId.Contains(w.CaseColorId) || filters.CaseColorId.Count == 0) &&
                            (filters.StrapColorId.Contains(w.StrapColorId) || filters.StrapColorId.Count == 0) &&
                            (filters.DialColorId.Contains(w.DialColorId) || filters.DialColorId.Count == 0) &&
                            (filters.WaterResistanceId.Contains(w.WaterResistanceId) || filters.WaterResistanceId.Count == 0) &&
                            (filters.IncrustationTypeId.Contains(w.IncrustationTypeId) || filters.IncrustationTypeId.Count == 0) &&
                            (filters.DialTypeId.Contains(w.DialTypeId) || filters.DialTypeId.Count == 0) &&
                            (filters.GenderId.Contains(w.GenderId) || filters.GenderId.Count == 0) &&
                            (filters.OnSale.Contains(w.OnSale) || filters.OnSale.Count == 0) &&
                            (filters.IsTop.Contains(w.IsTop) || filters.IsTop.Count == 0) &&
                            (w.Price >= filters.MinPrice || filters.MinPrice == null) &&
                            (w.Price <= filters.MaxPrice || filters.MaxPrice == null));
            }

            var result = new ResultModel<List<WatchModel>>()
            {
                Hits = watches.Count()
            };

            //watches = watches
            //    .Skip((page - 1) * perPage)
            //    .Take(perPage)
            //    .Include(w => w.Brand)
            //    .Include(w => w.Collection)
            //    .Include(w => w.Style)
            //    .Include(w => w.MovementType)
            //    .Include(w => w.GlassType)
            //    .Include(w => w.CaseShape)
            //    .Include(w => w.CaseMaterial)
            //    .Include(w => w.StrapType)
            //    .Include(w => w.CaseColor)
            //    .Include(w => w.StrapColor)
            //    .Include(w => w.DialColor)
            //    .Include(w => w.WaterResistance)
            //    .Include(w => w.IncrustationType)
            //    .Include(w => w.DialType)
            //    .Include(w => w.Gender);

            watches = watches
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Include(w => w.Images);

            result.Value = watches.ToList();

            return result;
        }


        // TODO Check SQL. Write store procedure
        // TODO Include only needed for preview

        public async Task<ResultModel<List<WatchModel>>> GetAsync(int page, int perPage, WatchFilterModel? filters = null)
        {
            return await Task.FromResult<ResultModel<List<WatchModel>>>(GetWatches(page, perPage, filters));
        }

        public async Task<IEnumerable<WatchModel>> Where(Func<WatchModel, bool> predicate)
        {
            return await Task.FromResult<IEnumerable<WatchModel>>(_db.Watches.Where(predicate));
        }

        // TODO remove comments

        //public async Task<IEnumerable<WatchModel>> GetAsync(string? model,
        //                                                       List<int>? producerIds = null,
        //                                                       decimal? minPrice = null,
        //                                                       decimal? maxPrice = null,
        //                                                       bool? onSale = null,
        //                                                       bool? isPopular = null)
        //{
        //    var watches = _db.Watches.Include(w => w.Brand).ToList();

        //    if(model != null)
        //    {
        //        model = model.ToLower();
        //        watches = watches
        //            .Where(w => w.Model.ToLower().Contains(model) || w.Brand != null && w.Brand.BrandName.ToLower().Contains(model))
        //            .ToList();
        //    }

        //    //if (producerIds != null)
        //    //{
        //    //    watches = watches.Where(w => producerIds.Contains(w.BrandId)).ToList();
        //    //}

        //    if (minPrice != null)
        //    {
        //        watches = watches.Where(w => w.Price >= minPrice).ToList();
        //    }

        //    if (maxPrice != null)
        //    {
        //        watches = watches.Where(w => w.Price <= maxPrice).ToList();
        //    }

        //    if (onSale != null)
        //    {
        //        watches = watches.Where(w => w.OnSale == onSale).ToList();
        //    }

        //    if (isPopular != null)
        //    {
        //        watches = watches.Where(w => w.IsTop == isPopular).ToList();
        //    }


        //    watches = watches.ToList();

        //    return watches;
        //}
    }
}
