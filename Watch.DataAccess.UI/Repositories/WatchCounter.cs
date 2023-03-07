using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class WatchCounter : IWatchCounter
    {
        private readonly UnitOfWorks.UnitOfWorks _uow;

        public WatchCounter(UnitOfWorks.UnitOfWorks uow)
        {
            _uow = uow;
        }
        public Task<int> Brand(int id)
        {
            return _uow.Watches.Count.Brand(id);
        }

        public Task<int> CaseColor(int id)
        {
            return _uow.Watches.Count.CaseColor(id);
        }

        public Task<int> CaseMaterial(int id)
        {
            return _uow.Watches.Count.CaseMaterial(id);
        }

        public Task<int> CaseShape(int id)
        {
            return _uow.Watches.Count.CaseShape(id);
        }

        public Task<int> Collection(int id)
        {
            return _uow.Watches.Count.Collection(id);
        }

        public Task<int> Country(int id)
        {
            return _uow.Watches.Count.Country(id);
        }

        public Task<int> DialColor(int id)
        {
            return _uow.Watches.Count.DialColor(id);
        }

        public Task<int> DialType(int id)
        {
            return _uow.Watches.Count.DialType(id);
        }

        public Task<int> Gender(int id)
        {
            return _uow.Watches.Count.Gender(id);
        }

        public Task<int> GlassType(int id)
        {
            return _uow.Watches.Count.GlassType(id);
        }

        public Task<int> IncrustationType(int id)
        {
            return _uow.Watches.Count.IncrustationType(id);
        }
        public Task<int> Function(int id)
        {
            return _uow.Watches.Count.Function(id);
        }


        public Task<int> MovementType(int id)
        {
            return _uow.Watches.Count.MovementType(id);
        }

        public Task<int> StrapColor(int id)
        {
            return _uow.Watches.Count.StrapColor(id);
        }

        public Task<int> StrapType(int id)
        {
            return _uow.Watches.Count.StrapType(id);
        }

        public Task<int> Style(int id)
        {
            return _uow.Watches.Count.Style(id);
        }

        public Task<int> WaterResistance(int id)
        {
            return _uow.Watches.Count.WaterResistance(id);
        }
    }
}
