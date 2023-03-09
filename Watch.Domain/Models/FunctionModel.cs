using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class FunctionModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public virtual ICollection<WatchModel> Watches { get; } = new List<WatchModel>();
    }
}
