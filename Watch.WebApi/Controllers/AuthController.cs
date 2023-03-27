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
using Watch.WebApi.Helpers;

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
        public async Task<Result<User>> Login([FromBody] LoginModel model)
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("IsActive", user.IsActive.ToString())
                };

                roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<User>()
                {
                    Token = token,
                    Value = new User(user)
                };
            }

            throw new InvalidCredentialsException();
        }

        [HttpPost("user")]
        public async Task<Result<User?>> Register([FromBody] RegisterModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim("IsActive", true.ToString())
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<User?>()
            {
                Token = token,
                Value = user
            };
        }

        [HttpPost("manager")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<User?>> RegisterManager([FromBody] RegisterModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Manager });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager),
                    new Claim("IsActive", true.ToString())
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<User?>()
            {
                Token = token,
                Value = user
            };
        }

        [HttpPost("admin")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<User?>> RegisterAdmin([FromBody] RegisterModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Admin });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager),
                    new Claim(ClaimTypes.Role, UserRoles.Admin),
                    new Claim("IsActive", true.ToString())
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<User?>()
            {
                Token = token,
                Value = user
            };
        }

    }
}
