using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch.Domain.Models;

namespace Watch.WebApi.Message
{
    public interface IVerification
    {
        Task SendConfirmationEmailAsync(ControllerBase controller, IConfiguration configuration, UserModel user);
        Task SendResetPasswordEmailAsync(ControllerBase controller, IConfiguration configuration, UserManager<UserModel> userManager, string email);
        bool CheckEmailConfirmation(UserModel user, string type, string token);
    }
}
