using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Exceptions;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpPost("")]
        public async Task<Result<string>> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.CheckCredentialsAsync(model);

            if (user != null)
            {
                if(!user.IsActive)
                {
                    throw new InactiveUserException(user.Id);
                }

                var roles = (await _context.Users.GetRolesAsync(user)).ToList();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<string>()
                {
                    Token = token,
                    Value = token
                };
            }

            throw new InvalidCredentialsException();
        }

        [HttpPost("user")]
        public async Task<Result<string>> Register([FromBody] RegisterModel model)
        {
            await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User)
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<string>()
            {
                Token = token,
                Value = token
            };
        }

        [HttpPost("manager")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<string>> RegisterManager([FromBody] RegisterModel model)
        {
            await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Manager });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager)
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<string>()
            {
                Token = token,
                Value = token
            };
        }

        [HttpPost("admin")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<string>> RegisterAdmin([FromBody] RegisterModel model)
        {
            await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Admin });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager),
                    new Claim(ClaimTypes.Role, UserRoles.Admin)
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<string>()
            {
                Token = token,
                Value = token
            };
        }

    }
}
