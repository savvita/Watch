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
    [Route("api/warehouses")]
    public class WarehouseController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public WarehouseController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Warehouse>>> Get([FromQuery]string cityRef)
        {
            var values = await _context.Warehouses.GetBySettlementRefAsync(cityRef);

            return new Result<List<Warehouse>>
            {
                Value = values,
                Hits = values.Count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("update")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<DateTime?>> GetLastUpdate()
        {
            var res = await _context.Warehouses.GetLastUpdateAsync();

            return new Result<DateTime?>
            {
                Value = res,
                Hits = 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Update()
        {
            bool res = false;

            if(_configuration["NP:ApiKey"] != null && _configuration["NP:Url"] != null)
            {
                res = await _context.Warehouses.UpdateAsync(_configuration["NP:ApiKey"]!, _configuration["NP:Url"]!);
            }

            return new Result<bool>
            {
                Value = res,
                Hits = 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

    }
}
