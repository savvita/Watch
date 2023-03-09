using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Material
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public Material()
        {
        }

        public Material(MaterialModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator MaterialModel(Material entity)
        {
            return new MaterialModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
