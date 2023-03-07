using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class IncrustationType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public IncrustationType()
        {
        }

        public IncrustationType(IncrustationTypeModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator IncrustationTypeModel(IncrustationType entity)
        {
            return new IncrustationTypeModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

    }
}
