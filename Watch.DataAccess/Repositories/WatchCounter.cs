using Watch.Domain.Interfaces;

namespace Watch.DataAccess.Repositories
{
    public class WatchCounter : IWatchCounter
    {
        private readonly WatchDbContext _db;
        public WatchCounter(WatchDbContext db)
        {
            _db = db;
        }

        public Task<int> Brand(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.BrandId == id));
        }

        public Task<int> CaseColor(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.CaseColorId == id));
        }

        public Task<int> CaseMaterial(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.CaseMaterialId == id));
        }

        public Task<int> CaseShape(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.CaseShapeId == id));
        }

        public Task<int> Collection(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.CollectionId == id));
        }

        public Task<int> Country(int id)
        {
            var brands = _db.Brands.Where(b => b.CountryId == id).Select(b => b.Id).ToList();
            return Task.FromResult<int>(_db.Watches.Count(w => w.BrandId != null && brands.Contains((int)w.BrandId)));
        }

        public Task<int> DialColor(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.DialColorId == id));
        }

        public Task<int> DialType(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.DialTypeId == id));
        }

        public Task<int> Function(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.Functions.FirstOrDefault(f => f.Id == id) != null));
        }

        public Task<int> Gender(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.GenderId == id));
        }

        public Task<int> GlassType(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.GlassTypeId == id));
        }

        public Task<int> IncrustationType(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.IncrustationTypeId == id));
        }

        public Task<int> MovementType(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.MovementTypeId == id));
        }

        public Task<int> StrapColor(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.StrapColorId == id));
        }

        public Task<int> StrapType(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.StrapTypeId == id));
        }

        public Task<int> Style(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.StyleId == id));
        }

        public Task<int> WaterResistance(int id)
        {
            return Task.FromResult<int>(_db.Watches.Count(w => w.WaterResistanceId == id));
        }
    }
}
