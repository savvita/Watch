using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        public string? PhoneNumber{ get; set; }

        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
    }
}
