using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class MovementType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public MovementType()
        {
        }

        public MovementType(MovementTypeModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator MovementTypeModel(MovementType entity)
        {
            return new MovementTypeModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
