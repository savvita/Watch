using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/colors")]
    public class ColorController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public ColorController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Color>>> Get()
        {
            var values = await _context.Colors.GetAsync();

            return new Result<List<Color>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{type}/{id:int}")]
        public async Task<Result<Color?>> Get(string type, int id)
        {
            var res = await _context.Colors.GetAsync(id);
            int count = 0;
            switch (type)
            {
                case "case":
                    count = await _context.Watches.Count.CaseColor(id);
                    break;

                case "dial":
                    count = await _context.Watches.Count.DialColor(id);
                    break;

                case "strap":
                    count = await _context.Watches.Count.StrapColor(id);
                    break;
            }
            return new Result<Color?>
            {
                Value = res,
                Hits = count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Color>> Create([FromBody] Color entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Colors.CreateAsync(entity);
            return new Result<Color>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Color?>> Update([FromBody] Color entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Color?>
            {
                Value = await _context.Colors.UpdateAsync(entity),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Colors.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
