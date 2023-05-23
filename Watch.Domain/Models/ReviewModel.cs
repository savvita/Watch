using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public int WatchId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;
        public int? ReplyToId { get; set; }
        public bool Checked { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public virtual ReviewModel? ReplyTo { get; set; }
        public virtual WatchModel? Watch { get; set; }
    }
}
