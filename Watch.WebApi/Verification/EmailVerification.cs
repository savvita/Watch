using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using Watch.Domain.Models;
using Watch.Email;
using Watch.WebApi.Helpers;
using Watch.WebApi.Verification;

namespace Watch.WebApi.Message
{
    public class EmailVerification : IVerification
    {
        private List<Claim> GetClaims(UserModel user, string type)
        {
            return new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.UserData, user.SecurityStamp),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Typ, type)
                };
        }
        public async Task SendConfirmationEmailAsync(ControllerBase controller, IConfiguration configuration, UserModel user)
        {
            var token = JwtHelper.GetToken(GetClaims(user, VerificationTypes.ConfirmEmail), configuration);
            var code = new JwtSecurityTokenHandler().WriteToken(token);

            var host = configuration["Email:Host"];
            var portStr = configuration["Email:Port"];
            var from = configuration["Email:From"];

            if (host == null || portStr == null || from == null)
            {
                return;
            }

            var port = int.Parse(portStr);

            EmailClient client = new EmailClient(host, port);

            string subject = "Реєстрація на WatchShopMarket";

            string codeHtmlVersion = HttpUtility.UrlEncode(code);

            var callbackUrl = controller.Url.Action(
                "ConfirmEmail",
                "Auth",
                new { userId = user.Id, code = codeHtmlVersion },
                protocol: controller.HttpContext.Request.Scheme);

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>WatchShopMarket</h1>");
            sb.Append("Для підтвердження пошти перейдіть за посиланням ");
            sb.Append($"<a href='{callbackUrl}'>Підтвердити</a>");

            await client.SendEmail(from, user.Email, subject, sb.ToString());
        }

        public async Task SendResetPasswordEmailAsync(ControllerBase controller, IConfiguration configuration, UserManager<UserModel> userManager, string userName)
        { 

            var user = await userManager.FindByNameAsync(userName);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return;
            }

            var host = configuration["Email:Host"];
            var portStr = configuration["Email:Port"];
            var from = configuration["Email:From"];

            if (host == null || portStr == null || from == null)
            {
                return;
            }

            var port = int.Parse(portStr);

            EmailClient client = new EmailClient(host, port);

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var codeHtmlVersion = HttpUtility.UrlEncode(code);

            var callbackUrl = controller.Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = codeHtmlVersion }, protocol: controller.HttpContext.Request.Scheme);

            string subject = "Скидання паролю на WatchShopMarket";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>WatchShopMarket</h1>");
            sb.Append("Для скидання паролю перейдіть за посиланням ");
            sb.Append($"<a href='{callbackUrl}'>Скинути</a>");

            await client.SendEmail(from, user.Email, subject, sb.ToString());
        }

        public bool CheckEmailConfirmation(UserModel user, string type, string token)
        {
            token = HttpUtility.UrlDecode(token);
            var tokenS = JwtHelper.ReadToken(token);

            try
            {
                var name = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                if(name != user.UserName)
                {
                    return false;
                }

                var stamp = tokenS.Claims.First(claim => claim.Type == ClaimTypes.UserData).Value;
                if (stamp != user.SecurityStamp)
                {
                    return false;
                }

                var typeT = tokenS.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Typ).Value;
                if (typeT != type)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
