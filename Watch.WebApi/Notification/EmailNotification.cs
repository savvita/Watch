using System.Text;
using Watch.Domain.Models;
using Watch.Email;

namespace Watch.WebApi.Notification
{
    public class EmailNotification : INotification
    {
        private readonly EmailClient _client;
        private readonly string? _from;
        private readonly string? _domain;
        public EmailNotification(IConfiguration configuration)
        {
            var host = configuration["Email:Host"];
            var portStr = configuration["Email:Port"];
            _from = configuration["Email:From"];

            var port = int.Parse(portStr!);

            _client = new EmailClient(host!, port);

            _domain = configuration["Domain"];
        }
        public async Task SendChangePaswordNotificationAsync(UserModel user, DateTime date)
        {
            if (_from == null || _domain == null)
            {
                return;
            }

            string subject = "Зміна паролю на WatchShopMarket";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>WatchShopMarket</h1>");
            sb.Append($"<p>Ваш пароль було змінено {date.ToString()}</p>");
            sb.Append($"<a href='{_domain}'>У магазин</a>");

            await _client.SendEmail(_from, user.Email, subject, sb.ToString());
        }

        public async Task SendOrderNotificationAsync(UserModel user, OrderModel order)
        {
            if (_from == null || _domain == null)
            {
                return;
            }

            string subject = "Нове замовлення у магазині WatchShopMarket";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>WatchShopMarket</h1>");
            sb.Append($"<p>Номер вашого замовлення {order.Id}</p>");
            sb.Append($"<p>Переглянути замовлення можна у <a href='{_domain}orders/{order.Id}'>особистому кабінеті</a></p>");

            await _client.SendEmail(_from, user.Email, subject, sb.ToString());
        }

        public async Task SendRegistrationNotificationAsync(UserModel user)
        {
            if(_from == null || _domain == null)
            {
                return;
            }

            string subject = "Реєстрація на WatchShopMarket";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>WatchShopMarket</h1>");
            sb.Append("<p>Ви успішно зареєструвалися на сайті WatchShopMarket</p>");
            sb.Append("<p>Вдалих покупок</p>");
            sb.Append($"<a href='{_domain}'>У магазин</a>");

            await _client.SendEmail(_from, user.Email, subject, sb.ToString());
        }

        public async Task SendResetPaswordNotificationAsync(UserModel user, DateTime date)
        {
            if (_from == null || _domain == null)
            {
                return;
            }

            string subject = "Скидання паролю на WatchShopMarket";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>WatchShopMarket</h1>");
            sb.Append($"<p>Ваш пароль було скинуто {date.ToString()}</p>");
            sb.Append($"<a href='{_domain}'>У магазин</a>");

            await _client.SendEmail(_from, user.Email, subject, sb.ToString());
        }
    }
}
