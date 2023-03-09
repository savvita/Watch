using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class DialType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public DialType()
        {
        }

        public DialType(DialTypeModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator DialTypeModel(DialType entity)
        {
            return new DialTypeModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
