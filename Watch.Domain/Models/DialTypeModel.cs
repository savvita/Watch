using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class DialTypeModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;
    }
}
