using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public Country()
        {
        }

        public Country(CountryModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator CountryModel(Country entity)
        {
            return new CountryModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
