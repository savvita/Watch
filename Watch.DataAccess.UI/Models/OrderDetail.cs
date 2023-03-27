namespace Watch.DataAccess.UI.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int WatchId { get; set; }

        public decimal UnitPrice{ get; set; }

        public int Count { get; set; }
        public int OrderId { get; set; }
        public Watch? Watch { get; set; }

        public OrderDetail()
        {
        }

        public OrderDetail(OrderDetailModel model)
        {
            Id = model.Id;
            UnitPrice = model.UnitPrice;
            WatchId = model.WatchId;
            OrderId = model.OrderId;
            Count = model.Count;

            if(model.Watch != null)
            {
                Watch = new Watch(model.Watch);
            }
        }

        public static explicit operator OrderDetailModel(OrderDetail entity)
        {
            return new OrderDetailModel()
            {
                Id = entity.Id,
                OrderId = entity.OrderId,
                Count = entity.Count,
                UnitPrice = entity.UnitPrice,
                WatchId = entity.WatchId
            };
        }
    }
}
