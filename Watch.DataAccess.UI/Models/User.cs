using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class User : IdentityUser
    {
        public long? ChatId { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }


        public User()
        {
        }

        public User(UserModel model)
        {
            Id = model.Id;
            ChatId = model.ChatId;
            UserName = model.UserName;
            FirstName = model.FirstName;
            Email = model.Email;
        }

        public static explicit operator UserModel(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ChatId = user.ChatId,
                FirstName = user.FirstName
            };
        }
    }
}
