using System.ComponentModel.DataAnnotations;

namespace Watch.NovaPoshta.Models
{
    public class CityModel
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
    }
}
