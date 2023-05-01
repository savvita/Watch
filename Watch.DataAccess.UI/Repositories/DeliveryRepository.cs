using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public DeliveryRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Delivery?> CreateAsync(Delivery entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Deliveries.CreateAsync((DeliveryModel)entity);
            return model != null ? new Delivery(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Deliveries.DeleteAsync(id);
        }

        public async Task<IEnumerable<Delivery>> GetAsync()
        {
            return (await _db.Deliveries.GetAsync()).Select(model => new Delivery(model));
        }

        public async Task<Delivery?> GetAsync(int id)
        {
            var model = await _db.Deliveries.GetAsync(id);

            return model != null ? new Delivery(model) : null;
        }

        public async Task<Delivery> UpdateAsync(Delivery entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Deliveries.UpdateAsync((DeliveryModel)entity);

            return new Delivery(model);
        }
    }
}
