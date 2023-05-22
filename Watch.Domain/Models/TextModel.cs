using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watch.Domain.Models
{
    public class TextModel
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        [MaxLength(500)]
        public string Text { get; set; } = null!;
        public double Top { get; set; }
        public decimal Left { get; set; }
        public int FontSize { get; set; }

        [StringLength(9, MinimumLength = 7)]
        public string FontColor { get; set; } = "#000000";

        [ForeignKey("Slides")]
        public int? SlideId { get; set; }
        public virtual SlideModel? Slide { get; set; }
    }
}
