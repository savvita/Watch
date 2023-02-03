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
            var users = (await _context.Users.GetAsync()).ToList();
            return new Result<List<User>>
            {
                Value = users,
                Hits = users.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }


        [HttpPut("")]
        public async Task<Result<User>> Update([FromBody] User user)
        {
            return new Result<User>
            {
                Value = await _context.Users.UpdateAsync(user),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id}")]
        public async Task<Result<bool>> Delete(string id)
        {
            await _context.Users.DeleteAsync(id);
            return new Result<bool>
            {
                Value = true,
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
