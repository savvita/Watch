using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class UserModel : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? SecondName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public bool IsActive { get; set; }
    }
}
