using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Value { get; set; } = null!;

        public Country? Country { get; set; }

        public Brand()
        {
        }

        public Brand(BrandModel model)
        {
            Id = model.Id;
            Value = model.Value;
            if(model.Country != null) 
            {
                Country = new Country(model.Country);
            }
        }

        public static explicit operator BrandModel(Brand entity)
        {
            return new BrandModel()
            {
                Id = entity.Id,
                Value = entity.Value,
                CountryId = entity.Country != null ? entity.Country.Id : null
            };
        }
    }
}
