using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Gender
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Value { get; set; } = null!;

        public Gender()
        {
        }

        public Gender(GenderModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator GenderModel(Gender entity)
        {
            return new GenderModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
