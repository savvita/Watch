using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class DeliveryModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
