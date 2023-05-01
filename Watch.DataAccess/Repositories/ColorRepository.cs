using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class ColorRepository : GenericRepository<ColorModel>, IColorRepository
    {
        public ColorRepository(WatchDbContext db) : base(db)
        {
        }

        public async Task<List<SaleModel>> GetSalesAsync(string type)
        {
            List<SaleModel> res = new List<SaleModel>();

            switch (type.ToLower())
            {
                case "strap":
                    res = await GetStrapColorSales();
                    break;
                case "dial":
                    res = await GetDialColorSales();
                    break;
                case "case":
                    res = await GetCaseColorSales();
                    break;
            }

            return res;
        }

        private async Task<List<SaleModel>> GetStrapColorSales()
        {
            var watches = _db.Watches.Where(w => w.StrapColorId != null).Select(w => w.Id);

            var res = await Task.FromResult(_db.OrderDetails
                .Where(detail => detail.Order != null && (detail.Order.StatusId == 3 || detail.Order.StatusId == 7))
                .Where(detail => watches.Contains(detail.WatchId))
                .Include(detail => detail.Watch)
                .ThenInclude(watch => watch!.StrapColor)
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

        private async Task<List<SaleModel>> GetDialColorSales()
        {
            var watches = _db.Watches.Where(w => w.DialColorId != null).Select(w => w.Id);

            var res = await Task.FromResult(_db.OrderDetails
                .Where(detail => detail.Order != null && (detail.Order.StatusId == 3 || detail.Order.StatusId == 7))
                .Where(detail => watches.Contains(detail.WatchId))
                .Include(detail => detail.Watch)
                .ThenInclude(watch => watch!.DialColor)
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

        private async Task<List<SaleModel>> GetCaseColorSales()
        {
            var watches = _db.Watches.Where(w => w.CaseColorId != null).Select(w => w.Id);

            var res = await Task.FromResult(_db.OrderDetails
                .Where(detail => detail.Order != null && (detail.Order.StatusId == 3 || detail.Order.StatusId == 7))
                .Where(detail => watches.Contains(detail.WatchId))
                .Include(detail => detail.Watch)
                .ThenInclude(watch => watch!.CaseColor)
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
