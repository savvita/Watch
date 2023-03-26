using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Watch.DataAccess.UI.Exceptions;

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
    }
}
