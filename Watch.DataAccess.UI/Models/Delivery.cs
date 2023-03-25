using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; } = null!;
        public bool IsActive { get; set; }

        public Delivery()
        {
        }

        public Delivery(DeliveryModel model)
        {
            Id = model.Id;
            Value = model.Value;
            IsActive = model.IsActive;
        }

        public static explicit operator DeliveryModel(Delivery entity)
        {
            return new DeliveryModel()
            {
                Id = entity.Id,
                Value = entity.Value,
                IsActive = entity.IsActive,
            };
        }
    }
}
