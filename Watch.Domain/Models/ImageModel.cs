using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watch.Domain.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Value { get; set; } = null!;

        [ForeignKey("Watches")]
        public int WatchModelId { get; set; }

        public virtual WatchModel? Watch{ get; set; }
    }
}
