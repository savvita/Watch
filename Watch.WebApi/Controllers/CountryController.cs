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
    [Route("api/countries")]
    public class CountryController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public CountryController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Country>>> Get()
        {
            var values = await _context.Countries.GetAsync();

            return new Result<List<Country>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Country?>> Get(int id)
        {
            var res = await _context.Countries.GetAsync(id);
            return new Result<Country?>
            {
                Value = res,
                Hits = await _context.Watches.Count.Country(id),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Country>> Create([FromBody] Country entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Countries.CreateAsync(entity);
            return new Result<Country>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Country?>> Update([FromBody] Country entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Country?>
            {
                Value = await _context.Countries.UpdateAsync(entity),
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Countries.DeleteAsync(id);
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
            var res = await _context.Countries.GetSalesAsync();

            return new Result<List<Sale>>
            {
                Value = res,
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
