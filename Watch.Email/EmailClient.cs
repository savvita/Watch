using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Email
{
    public class EmailClient
    {
        private readonly string _host;
        private readonly int _port;

        public EmailClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task SendEmail(string from, string to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using SmtpClient client = new SmtpClient(_host, _port);
            await client.SendMailAsync(message);
        }
    }
}
