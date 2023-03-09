using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class StrapType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public StrapType()
        {
        }

        public StrapType(StrapTypeModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator StrapTypeModel(StrapType entity)
        {
            return new StrapTypeModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
