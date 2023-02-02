using Microsoft.AspNetCore.Identity;

namespace Watch.Domain.Models
{
    public class UserModel : IdentityUser
    {
        public long? ChatId { get; set; }
        public string? FirstName { get; set; }

    }
}
