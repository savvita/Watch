

using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class NewReview
    {
        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;
        public byte? Rate { get; set; }

    }
}
