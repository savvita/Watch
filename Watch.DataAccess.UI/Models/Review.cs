using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;
        [Required]
        public int WatchId { get; set; }
        public int? ReplyToId { get; set; }
        public bool Checked { get; set; } = false;
        public bool Deleted { get; set; } = false;

        public Review()
        {
        }

        public Review(ReviewModel model)
        {
            Id = model.Id;
            Date = model.Date;
            UserId = model.UserId;
            Text = model.Text;
            WatchId = model.WatchId;
            ReplyToId = model.ReplyToId;
            Checked = model.Checked;
            Deleted = model.Deleted;
            UserName = model.UserName;
        }

        public static explicit operator ReviewModel(Review entity)
        {
            return new ReviewModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                UserId = entity.UserId,
                WatchId = entity.WatchId,
                Text = entity.Text,
                ReplyToId = entity.ReplyToId,
                Checked = entity.Checked,
                UserName = entity.UserName,
                Deleted = entity.Deleted
            };
        }
    }
}
