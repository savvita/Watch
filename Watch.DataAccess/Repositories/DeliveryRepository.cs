using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class DeliveryRepository : GenericRepository<DeliveryModel>, IDeliveryRepository
    {
        public DeliveryRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            var model = await base.GetAsync(id);
            if (model == null)
            {
                return false;
            }

            model.IsActive = false;

            await base.UpdateAsync(model);
            return true;
        }
    }
}
