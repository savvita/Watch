using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Color
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Value { get; set; } = null!;

        public Color()
        {
        }

        public Color(ColorModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator ColorModel(Color entity)
        {
            return new ColorModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
