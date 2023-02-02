using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int WatchId { get; set; }

        public decimal UnitPrice{ get; set; }

        public int Count { get; set; }
        public int OrderId { get; set; }

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
        }

        public static explicit operator OrderDetailModel(OrderDetail detail)
        {
            return new OrderDetailModel()
            {
                Id = detail.Id,
                OrderId = detail.OrderId,
                Count = detail.Count,
                UnitPrice = detail.UnitPrice,
                WatchId = detail.WatchId
            };
        }
    }
}
