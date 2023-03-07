using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; } = null!;

        public OrderStatus()
        {
        }

        public OrderStatus(OrderStatusModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator OrderStatusModel(OrderStatus entity)
        {
            return new OrderStatusModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
