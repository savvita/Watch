using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class GenderModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Value { get; set; } = null!;
    }
}
