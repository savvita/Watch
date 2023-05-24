using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class User
    {
        public string Id { get; set; } = null!;
        [Timestamp]
        public byte[]? RowVersion { get; set; } = null!;

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? SecondName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool IsManager { get; set; }

        public bool IsAdmin { get; set; }

        public User()
        {
        }

        public User(UserModel model)
        {
            Id = model.Id;
            UserName = model.UserName;
            FirstName = model.FirstName;
            SecondName = model.SecondName;
            LastName = model.LastName;
            Email = model.Email;
            IsActive = model.IsActive;
            PhoneNumber = model.PhoneNumber;
            RowVersion = model.RowVersion;
            EmailConfirmed = model.EmailConfirmed;
        }

        public static explicit operator UserModel(User entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                SecondName = entity.SecondName,
                LastName = entity.LastName,
                IsActive = entity.IsActive,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                RowVersion = entity.RowVersion,
                EmailConfirmed = entity.EmailConfirmed
            };
        }
    }
}
