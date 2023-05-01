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
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<List<User>>> Get()
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var users = (await _context.Users.GetAsync()).ToList();
            return new Result<List<User>>
            {
                Value = users,
                Hits = users.Count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<Result<User?>> Get(string username)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            if(User.Identity == null || User.Identity.Name != username && !User.IsInRole(UserRoles.Admin))
            {
                throw new ForbiddenException();
            }

            var user = await _context.Users.GetByUserNameAsync(username);

            return new Result<User?>
            {
                Value = user,
                Hits = user != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }


        [HttpPut("")]
        [Authorize]
        public async Task<Result<User>> Update([FromBody] User entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);


            if (User.Identity == null)
            {
                throw new ForbiddenException();
            }

            if(!User.IsInRole(UserRoles.Admin))
            {
                if(User.Identity.Name == null)
                {
                    throw new ForbiddenException();
                }

                var userToUpdate = await _context.Users.GetAsync(entity.Id);

                if (userToUpdate == null)
                {
                    throw new UserNotFoundException(entity.Id);
                }

                var user = await _context.Users.GetByUserNameAsync(User.Identity.Name);

                if (user == null || user.Id != userToUpdate.Id)
                {
                    throw new ForbiddenException();
                }
            }

            var res = await _context.Users.UpdateAsync(entity, User.IsInRole(UserRoles.Admin));


            return new Result<User>
            {
                Value = res,
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("restore/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Restore(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Users.RestoreAsync(id);

            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<Result<bool>> Delete(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            if (User.Identity == null)
            {
                throw new ForbiddenException();
            }

            if (!User.IsInRole(UserRoles.Admin))
            {
                if (User.Identity.Name == null)
                {
                    throw new ForbiddenException();
                }

                var userToUpdate = await _context.Users.GetAsync(id);

                if (userToUpdate == null)
                {
                    throw new UserNotFoundException(id);
                }

                var user = await _context.Users.GetByUserNameAsync(User.Identity.Name);

                if (user == null || user.Id != userToUpdate.Id)
                {
                    throw new ForbiddenException();
                }
            }
            var res = await _context.Users.DeleteAsync(id);

            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        //private async Task<string> GetTokenAsync()
        //{
        //    var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

        //    if (username == null)
        //    {
        //        throw new InternalServerException();
        //    }

        //    var user = await _context.Users.GetByUserNameAsync(username.Value);

        //    if (user == null)
        //    {
        //        throw new UserNotFoundException(username.Value);
        //    }

        //    if (user.UserName == null)
        //    {
        //        throw new InternalServerException();
        //    }

        //    var roles = (await _context.Users.GetRolesAsync((UserModel)user)).ToList();

        //    var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim("IsActive", user.IsActive.ToString())
        //        };

        //    roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        //    return new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));
        //}
    }
}
