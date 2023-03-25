using System.ComponentModel.DataAnnotations;
using Watch.NovaPoshta.Models;

namespace Watch.Domain.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = null!;
        public int StatusId { get; set; }
        public string? ManagerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        public int PaymentId { get; set; }
        public int DeliveryId { get; set; }
        public string? CityId { get; set; }
        public string? WarehouseId { get; set; }
        [MaxLength(36)]
        public string? EN { get; set; }
        public virtual OrderStatusModel? Status { get; set; }
        public virtual ICollection<OrderDetailModel> Details { get; } = new List<OrderDetailModel>();
        public virtual UserModel? Manager { get; set; }
        public virtual PaymentModel? Payment { get; set; }
        public virtual DeliveryModel? Delivery { get; set; }
        public virtual CityModel? City { get; set; }
        public virtual WarehouseModel? Warehouse { get; set; }
        
    }
}
