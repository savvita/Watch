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
    [Route("api/movementtypes")]
    public class MovementTypeController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public MovementTypeController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<MovementType>>> Get()
        {
            var values = await _context.MovementTypes.GetAsync();

            return new Result<List<MovementType>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<MovementType?>> Get(int id)
        {
            var res = await _context.MovementTypes.GetAsync(id);
            return new Result<MovementType?>
            {
                Value = res,
                Hits = await _context.Watches.Count.MovementType(id),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<MovementType>> Create([FromBody] MovementType entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.MovementTypes.CreateAsync(entity);
            return new Result<MovementType>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<MovementType?>> Update([FromBody] MovementType entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<MovementType?>
            {
                Value = await _context.MovementTypes.UpdateAsync(entity),
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.MovementTypes.DeleteAsync(id);
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
            var res = await _context.MovementTypes.GetSalesAsync();

            return new Result<List<Sale>>
            {
                Value = res,
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
