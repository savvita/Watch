using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class OrderAdditionalInfo
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

        public static explicit operator OrderAdditionalInfoModel(OrderAdditionalInfo entity)
        {
            return new OrderAdditionalInfoModel()
            {
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                PaymentId = entity.PaymentId,
                DeliveryId = entity.DeliveryId,
                SettlementRef = entity.SettlementRef,
                WarehouseRef = entity.WarehouseRef
            };
        }
    }
}
