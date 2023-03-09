namespace Watch.DataAccess.UI.Models
{
    public class BasketDetail
    {
        public int Id { get; set; }

        public int WatchId { get; set; }

        public int Count { get; set; }
        public int BasketId { get; set; }
        public decimal UnitPrice { get; set; }

        public BasketDetail()
        {
        }

        public BasketDetail(BasketDetailModel model)
        {
            Id = model.Id;
            WatchId = model.WatchId;
            BasketId = model.BasketId;
            Count = model.Count;
        }

        public static explicit operator BasketDetailModel(BasketDetail entity)
        {
            return new BasketDetailModel()
            {
                Id = entity.Id,
                BasketId = entity.BasketId,
                Count = entity.Count,
                WatchId = entity.WatchId
            };
        }
    }
}
