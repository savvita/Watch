using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
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
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
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
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
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
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
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
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("sales/{type}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Sale>>> GetSales(string type)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Colors.GetSalesAsync(type);

            return new Result<List<Sale>>
            {
                Value = res,
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
