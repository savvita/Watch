using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class CityModel
    {
        [Key]
        [Required]
        [MaxLength(36)]
        public string Ref { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string AreaDescription { get; set; } = null!;

        [Required]
        public DateTime LastUpdate { get; set; }

        public CityModel()
        {

        }

        public CityModel(NovaPoshta.Models.CityModel model)
        {
            Ref = model.Ref;
            Description = model.Description;
            AreaDescription = model.AreaDescription;
            LastUpdate = DateTime.Now;
        }
    }
}
