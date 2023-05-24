using Microsoft.AspNetCore.Mvc;
using Watch.Domain.Models;

namespace Watch.WebApi.Message
{
    public interface IVerification
    {
        Task SendConfirmationEmailAsync(ControllerBase controller, IConfiguration configuration, UserModel user);
        bool CheckEmailConfirmation(UserModel user, string type, string token);
    }
}
