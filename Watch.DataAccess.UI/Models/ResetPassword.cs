using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class ResetPassword
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;
    }
}
