using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class ChangePassword
    {
        [Required]
        public string NewPassword { get; set; } = null!;
        [Required]
        public string OldPassword { get; set; } = null!;
    }
}
