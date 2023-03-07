using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class BrandModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;
        public int? CountryId { get; set; }
        public virtual CountryModel? Country{ get; set; }
    }
}
