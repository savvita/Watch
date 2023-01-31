using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public OrderStatus? Status { get; set; }

        public DateTime Date { get; set; }

        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        public Order()
        {
        }

        public Order(OrderModel model)
        {
            Id = model.Id;
            Date = model.Date;
            UserId = model.UserId;

            if (model.Status != null)
            {
                Status = new OrderStatus(model.Status);
            }

        }

        public static explicit operator OrderModel(Order order)
        {
            var model = new OrderModel()
            {
                Id = order.Id,
                Date = order.Date,
                StatusId = order.Status != null ? order.Status.Id : 0,
                UserId = order.UserId
            };

            order.Details.ForEach(detail => model.Details.Add((OrderDetailModel)detail));
            return model;
        }
    }
}
