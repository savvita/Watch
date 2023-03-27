using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<OrderModel?> GetAsync(int id)
        {
            var order = await _db.Orders
                .Where(x => x.Id == id)
                .Include(x => x.City)
                .Include(x => x.Warehouse)
                .Include(x => x.Status)
                .Include(x => x.Delivery)
                .Include(x => x.Payment)
                .Include(x => x.Details)
                .FirstOrDefaultAsync();

            if(order == null)
            {
                return null;
            }

            foreach(var detail in order.Details)
            {
                detail.Watch = await _db.Watches.FirstOrDefaultAsync(x => x.Id == detail.WatchId);
            }

            return order;
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }

        public async Task<IEnumerable<OrderModel>> GetByManagerIdAsync(string managerId)
        {
            return await Task.FromResult<IEnumerable<OrderModel>>(_db.Orders.Where(o => o.ManagerId == managerId));
        }

        public async Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult<IEnumerable<OrderModel>>(_db.Orders.Where(o => o.UserId == userId));
        }

        public async Task<bool> SetOrderStatusAsync(int id, int statusId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null || order.StatusId == 3 || order.StatusId == 4)
            {
                return false;
            }

            order.StatusId = statusId;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var res = await SetOrderStatusAsync(id, 4);
            if(res == false)
            {
                return false;
            }

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if(order == null)
            {
                return false;
            }

            foreach(var detail in order.Details)
            {
                var watch = await _db.Watches.FirstOrDefaultAsync(w => w.Id == detail.WatchId);

                if(watch != null)
                {
                    watch.Available += detail.Count;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseOrderAsync(int id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null || order.StatusId == 3 || order.StatusId == 4)
            {
                return false;
            }

            order.StatusId = 3;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptOrderAsync(int orderId, string managerId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null || order.StatusId != 1)
            {
                return false;
            }

            if(order.ManagerId != null)
            {
                return false;
            }

            order.StatusId = 2;
            order.ManagerId = managerId;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderModel?>> GetAsync(OrderFilterModel filters)
        {
            var watches = _db.Watches
                .Where(w => (w.BrandId == filters.BrandId || filters.BrandId == null) &&
                            (w.Id == filters.WatchId || filters.WatchId == null) &&
                            (w.CollectionId == filters.CollectionId || filters.CollectionId == null) &&
                            (w.StyleId == filters.StyleId || filters.StyleId == null) &&
                            (w.MovementTypeId == filters.MovementTypeId || filters.MovementTypeId == null) &&
                            (w.GlassTypeId == filters.GlassTypeId || filters.GlassTypeId == null) &&
                            (w.CaseShapeId == filters.CaseShapeId || filters.CaseShapeId == null) &&
                            (w.CaseMaterialId == filters.CaseMaterialId || filters.CaseMaterialId == null) &&
                            (w.StrapTypeId == filters.StrapTypeId || filters.StrapTypeId == null) &&
                            (w.CaseColorId == filters.CaseColorId || filters.CaseColorId == null) &&
                            (w.StrapColorId == filters.StrapColorId || filters.StrapColorId == null) &&
                            (w.DialColorId == filters.DialColorId || filters.DialColorId == null) &&
                            (w.WaterResistanceId == filters.WaterResistanceId || filters.WaterResistanceId == null) &&
                            (w.IncrustationTypeId == filters.IncrustationTypeId || filters.IncrustationTypeId == null) &&
                            (w.DialTypeId == filters.DialTypeId || filters.DialTypeId == null) &&
                            (w.GenderId == filters.GenderId || filters.GenderId == null));

            if(filters.CountryId != null)
            {
                var brands = _db.Brands.Where(b => b.CountryId == filters.CountryId).Select(b => b.Id).ToList();
                watches = watches.Where(x => x.BrandId != null && brands.Contains((int)x.BrandId));
            }

            if (filters.FunctionId != null)
            {
                var function = await _db.Functions.FirstOrDefaultAsync(f => f.Id == filters.FunctionId);

                if(function != null)
                {
                    watches = watches.Where(x => x.Functions.Contains(function));
                }
            }

            var w = watches.Select(w => w.Id);

            var res = await Task.FromResult(_db.OrderDetails
                .Where(o => w.Contains(o.WatchId))
                .Include(x => x.Order)
                .ThenInclude(x => x.Details)
                .Select(x => x.Order)
                .ToList());
            return res;
        }
    }
}
