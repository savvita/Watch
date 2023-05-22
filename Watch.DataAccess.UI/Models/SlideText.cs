using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class SlideText
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        [MaxLength(500)]
        public string Text { get; set; } = null!;
        public double Top { get; set; }
        public decimal Left { get; set; }
        public int? SlideId { get; set; }
        public int FontSize { get; set; }

        [StringLength(9, MinimumLength = 7)]
        public string FontColor { get; set; } = "#000000";
        public SlideText()
        {
        }

        public SlideText(TextModel model)
        {
            Id = model.Id;
            Text = model.Text;
            Left = model.Left;
            Top = model.Top;
            SlideId = model.SlideId;
            FontColor = model.FontColor;
            FontSize = model.FontSize;
            RowVersion = model.RowVersion;
        }

        public static explicit operator TextModel(SlideText entity)
        {
            return new TextModel()
            {
                Id = entity.Id,
                Text = entity.Text,
                Left = entity.Left,
                Top = entity.Top,
                SlideId = entity.SlideId,
                FontSize = entity.FontSize,
                FontColor = entity.FontColor,
                RowVersion = entity.RowVersion
            };
        }
    }
}
