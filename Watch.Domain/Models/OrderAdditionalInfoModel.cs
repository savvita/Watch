using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class OrderAdditionalInfoModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        public int PaymentId { get; set; }
        public int DeliveryId { get; set; }
        public string? SettlementRef { get; set; }
        public string? WarehouseRef { get; set; }
        [MaxLength(500)]
        public string? Comments { get; set; }
    }
}
