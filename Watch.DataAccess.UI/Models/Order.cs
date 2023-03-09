namespace Watch.DataAccess.UI.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public OrderStatus? Status { get; set; }

        public DateTime Date { get; set; }
        public User? Manager { get; set; }

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

            if(model.Manager != null)
            {
                Manager = new User(model.Manager);
            }

        }

        public static explicit operator OrderModel(Order entity)
        {
            var model = new OrderModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                StatusId = entity.Status != null ? entity.Status.Id : 0,
                ManagerId = entity.Manager != null ? entity.Manager.Id : null,
                UserId = entity.UserId,
            };

            entity.Details.ForEach(detail => model.Details.Add((OrderDetailModel)detail));
            return model;
        }
    }
}
