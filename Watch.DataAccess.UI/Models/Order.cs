using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public string UserId { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        public OrderStatus? Status { get; set; }

        public DateTime Date { get; set; }
        public User? Manager { get; set; }
        public Payment? Payment { get; set; }
        public Delivery? Delivery { get; set; }
        public City? City { get; set; }
        public Warehouse? Warehouse { get; set; }
        [MaxLength(36)]
        public string? EN { get; set; }

        [MaxLength(500)]
        public string? Comments { get; set; }

        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        public Order()
        {
        }

        public Order(OrderModel model)
        {
            Id = model.Id;
            Date = model.Date;
            UserId = model.UserId;
            EN = model.EN;
            FullName = model.FullName;
            PhoneNumber = model.PhoneNumber;
            Comments = model.Comments;
            RowVersion = model.RowVersion;

            if (model.Status != null)
            {
                Status = new OrderStatus(model.Status);
            }

            if(model.Manager != null)
            {
                Manager = new User(model.Manager);
            }

            if(model.Delivery != null)
            {
                Delivery = new Delivery(model.Delivery);
            }

            if (model.Payment != null)
            {
                Payment = new Payment(model.Payment);
            }

            if(model.City != null)
            {
                City = new City(model.City);
            }

            if(model.Warehouse != null)
            {
                Warehouse = new Warehouse(model.Warehouse);
            }

            model.Details.ToList().ForEach(x => Details.Add(new OrderDetail(x)));
        }

        public static explicit operator OrderModel(Order entity)
        {
            var model = new OrderModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                StatusId = entity.Status != null ? entity.Status.Id : 0,
                ManagerId = entity.Manager != null ? entity.Manager.Id : null,
                PaymentId = entity.Payment != null ? entity.Payment.Id : 0,
                DeliveryId = entity.Delivery != null ? entity.Delivery.Id : 0,
                UserId = entity.UserId,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                CityId = entity.City != null ? entity.City.Ref : null,
                WarehouseId = entity.Warehouse != null ? entity.Warehouse.Ref : null,
                EN = entity.EN,
                Comments = entity.Comments,
                RowVersion = entity.RowVersion
            };

            entity.Details.ForEach(detail => model.Details.Add((OrderDetailModel)detail));
            return model;
        }
    }
}
