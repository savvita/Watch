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
    [Route("api/styles")]
    public class StyleController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public StyleController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Style>>> Get()
        {
            var values = await _context.Styles.GetAsync();

            return new Result<List<Style>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Style?>> Get(int id)
        {
            var res = await _context.Styles.GetAsync(id);
            return new Result<Style?>
            {
                Value = res,
                Hits = await _context.Watches.Count.Style(id),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Style>> Create([FromBody] Style entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Styles.CreateAsync(entity);
            return new Result<Style>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Style?>> Update([FromBody] Style entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Style?>
            {
                Value = await _context.Styles.UpdateAsync(entity),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Styles.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
