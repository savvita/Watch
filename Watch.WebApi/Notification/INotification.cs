using Microsoft.AspNetCore.Mvc;
using Watch.Domain.Models;

namespace Watch.WebApi.Notification
{
    public interface INotification
    {
        Task SendRegistrationNotificationAsync(UserModel user);
        Task SendResetPaswordNotificationAsync(UserModel user, DateTime date);
        Task SendChangePaswordNotificationAsync(UserModel user, DateTime date);
        Task SendOrderNotificationAsync(UserModel user, OrderModel order);
    }
}
