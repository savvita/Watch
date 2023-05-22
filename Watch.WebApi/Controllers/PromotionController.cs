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
    [Route("api/promotions")]
    public class PromotionController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public PromotionController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Promotion>>> Get()
        {
            var values = await _context.Promotions.GetAsync(true);

            return new Result<List<Promotion>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Promotion?>> Get(int id)
        {
            var value = await _context.Promotions.GetAsync(id);

            return new Result<Promotion?>
            {
                Value = value,
                Hits = value != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("all")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Promotion>>> GetAll()
        {
            var values = await _context.Promotions.GetAsync();

            return new Result<List<Promotion>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Promotion>> Create([FromBody] Promotion entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Promotions.CreateAsync(entity);
            return new Result<Promotion>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<ConcurrencyUpdateResult>> Update([FromBody] Promotion entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var res = await _context.Promotions.UpdateConcurrencyAsync(entity);

            return new Result<ConcurrencyUpdateResult>
            {
                Value = res,
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Promotions.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
