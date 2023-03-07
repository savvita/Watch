using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class WaterResistanceModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Value { get; set; } = null!;
    }
}
