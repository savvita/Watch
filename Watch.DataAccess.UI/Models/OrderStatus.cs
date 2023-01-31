using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StatusName { get; set; } = null!;

        public OrderStatus()
        {
        }

        public OrderStatus(OrderStatusModel model)
        {
            Id = model.Id;
            StatusName = model.StatusName;
        }

        public static explicit operator OrderStatusModel(OrderStatus status)
        {
            return new OrderStatusModel()
            {
                Id = status.Id,
                StatusName = status.StatusName
            };
        }
    }
}
