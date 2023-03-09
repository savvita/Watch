using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;

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
        public async Task<Result<User>> Update([FromBody] User user)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<User>
            {
                Value = await _context.Users.UpdateAsync(user),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("/restore/{id}")]
        public async Task<Result<bool>> Restore(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Users.RestoreAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id}")]
        public async Task<Result<bool>> Delete(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Users.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
