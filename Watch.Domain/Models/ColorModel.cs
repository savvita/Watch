using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class ColorModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Value { get; set; } = null!;
    }
}
