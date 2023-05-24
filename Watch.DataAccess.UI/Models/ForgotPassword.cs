using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class ForgotPassword
    {
        [Required]
        public string UserName { get; set; } = null!;
    }
}
