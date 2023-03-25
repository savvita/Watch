using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class City
    {
        [Required]
        [MaxLength(36)]
        public string Ref { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string AreaDescription { get; set; } = null!;

        public DateTime LastUpdate { get; set; }

        public City()
        {
        }

        public City(CityModel model)
        {
            Ref = model.Ref;
            Description = model.Description;
            AreaDescription = model.AreaDescription;
            LastUpdate = model.LastUpdate;
        }

        public static explicit operator CityModel(City entity)
        {
            return new CityModel()
            {
                Ref = entity.Ref,
                Description = entity.Description,
                AreaDescription = entity.AreaDescription,
            };
        }
    }
}
