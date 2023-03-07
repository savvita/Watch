using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Style
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public Style()
        {
        }

        public Style(StyleModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator StyleModel(Style entity)
        {
            return new StyleModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
