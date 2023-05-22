using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class SlideModel
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public int Index { get; set; }

        public int? PromotionId { get; set; }

        public virtual ICollection<TextModel> Texts { get; } = new List<TextModel>();
        public bool IsActive { get; set; } = false;
        public virtual PromotionModel? Promotion { get; set; }

    }
}
