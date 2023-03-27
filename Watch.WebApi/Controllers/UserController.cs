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
    [Route("api/users")]
    [Authorize(Roles = UserRoles.Admin)]

    public class UserController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<User>>> Get()
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var users = (await _context.Users.GetAsync()).ToList();
            return new Result<List<User>>
            {
                Value = users,
                Hits = users.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id}")]
        public async Task<Result<User?>> Get(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var user = await _context.Users.GetAsync(id);
            return new Result<User?>
            {
                Value = user,
                Hits = user != null ? 1 : 0,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }


        [HttpPut("")]
        public async Task<Result<User>> Update([FromBody] User entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var res = await _context.Users.UpdateAsync(entity);


            var token = await GetTokenAsync();

            return new Result<User>
            {
                Value = res,
                Hits = 1,
                Token = token
            };
        }

        [HttpPut("restore/{id}")]
        public async Task<Result<bool>> Restore(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Users.RestoreAsync(id);
            var token = await GetTokenAsync();
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = token
            };
        }

        [HttpDelete("{id}")]
        public async Task<Result<bool>> Delete(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Users.DeleteAsync(id);

            var token = await GetTokenAsync();
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = token
            };
        }

        private async Task<string> GetTokenAsync()
        {
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            if (user.UserName == null)
            {
                throw new InternalServerException();
            }

            var roles = (await _context.Users.GetRolesAsync((UserModel)user)).ToList();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("IsActive", user.IsActive.ToString())
                };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            return new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));
        }
    }
}
