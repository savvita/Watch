using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class MaterialRepository : GenericRepository<MaterialModel>, IMaterialRepository
    {
        public MaterialRepository(WatchDbContext db) : base(db)
        {
        }

        public async Task<List<SaleModel>> GetSalesAsync(string type)
        {
            List<SaleModel> res = new List<SaleModel>();

            switch (type.ToLower())
            {
                case "case":
                    res = await GetCaseMaterialSales();
                    break;
            }

            return res;
        }

        private async Task<List<SaleModel>> GetCaseMaterialSales()
        {
            var watches = _db.Watches.Where(w => w.CaseMaterialId != null).Select(w => w.Id);

            var res = await Task.FromResult(_db.OrderDetails
                .Where(detail => detail.Order != null && (detail.Order.StatusId == 3 || detail.Order.StatusId == 7))
                .Where(detail => watches.Contains(detail.WatchId))
                .Include(detail => detail.Watch)
                .ThenInclude(watch => watch!.CaseMaterial)
                .Select(item => new SaleModel
                {
                    Date = item.Order!.Date,
                    Watch = item.Watch!,
                    Count = item.Count,
                    UnitPrice = item.UnitPrice
                })
                .ToList());

            return res;
        }
    }
}
