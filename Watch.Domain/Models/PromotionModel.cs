using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class PromotionModel
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        public int? BrandId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal DiscountValue { get; set; } = 0;

        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public virtual BrandModel? Brand { get; set; }
    }
}
