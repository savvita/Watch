using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class BasketDetail
    {
        public int Id { get; set; }

        public int WatchId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Count { get; set; }
        public int BasketId { get; set; }

        public BasketDetail()
        {
        }

        public BasketDetail(BasketDetailModel model)
        {
            Id = model.Id;
            UnitPrice = model.UnitPrice;
            WatchId = model.WatchId;
            BasketId = model.BasketId;
            Count = model.Count;
        }

        public static explicit operator BasketDetailModel(BasketDetail detail)
        {
            return new BasketDetailModel()
            {
                Id = detail.Id,
                BasketId = detail.BasketId,
                Count = detail.Count,
                UnitPrice = detail.UnitPrice,
                WatchId = detail.WatchId
            };
        }
    }
}
