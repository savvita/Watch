using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Exceptions;
using Watch.Domain.Models;

namespace Watch.WebApi.Helpers
{
    public static class JwtHelper
    {
        public static JwtSecurityToken GetToken(IEnumerable<Claim> claimsList, IConfiguration configuration)
        {
            var secret = configuration["JWT:Secret"];

            if (secret == null)
            {
                throw new InternalServerException();
            }

            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(6),
                    claims: claimsList,
                    signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public static async Task<string?> GetTokenAsync(DbContext context, ClaimsPrincipal user, IConfiguration configuration)
        {
            var username = user.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                return null;
            }

            var _user = await context.Users.GetByUserNameAsync(username.Value);

            if (_user == null)
            {
                return null;
            }

            if (_user.UserName == null)
            {
                return null;
            }

            var roles = (await context.Users.GetRolesAsync((UserModel)_user)).ToList();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("IsActive", _user.IsActive.ToString())
                };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            return new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, configuration));
        }

        public static JwtSecurityToken ReadToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken;
        }
    }
}
