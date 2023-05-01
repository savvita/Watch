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
    [Route("api/waterresistances")]
    public class WaterResistanceController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public WaterResistanceController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<WaterResistance>>> Get()
        {
            var values = await _context.WaterResistance.GetAsync();

            return new Result<List<WaterResistance>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<WaterResistance?>> Get(int id)
        {
            var res = await _context.WaterResistance.GetAsync(id);
            return new Result<WaterResistance?>
            {
                Value = res,
                Hits = await _context.Watches.Count.WaterResistance(id),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<WaterResistance>> Create([FromBody] WaterResistance entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.WaterResistance.CreateAsync(entity);
            return new Result<WaterResistance>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<WaterResistance?>> Update([FromBody] WaterResistance entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<WaterResistance?>
            {
                Value = await _context.WaterResistance.UpdateAsync(entity),
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.WaterResistance.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("sales")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Sale>>> GetSales()
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.WaterResistance.GetSalesAsync();

            return new Result<List<Sale>>
            {
                Value = res,
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
