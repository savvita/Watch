using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class GlassType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public GlassType()
        {
        }

        public GlassType(GlassTypeModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator GlassTypeModel(GlassType entity)
        {
            return new GlassTypeModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
