using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.DataAccess.UI;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/deliveries")]
    public class DeliveryController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public DeliveryController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        //TODO check receiving all and only active
        [HttpGet("")]
        public async Task<Result<List<Delivery>>> Get([FromQuery] bool? all)
        {
            var values = await _context.Deliveries.GetAsync();
            if (all == null || all == false)
            {
                values = values.Where(x => x.IsActive).ToList();
            }

            return new Result<List<Delivery>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Delivery?>> Get(int id)
        {
            var res = await _context.Deliveries.GetAsync(id);
            return new Result<Delivery?>
            {
                Value = res,
                Hits = res == null ? 0 : 1,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<Delivery>> Create([FromBody] Delivery entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Deliveries.CreateAsync(entity);
            return new Result<Delivery>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<Delivery?>> Update([FromBody] Delivery entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Delivery?>
            {
                Value = await _context.Deliveries.UpdateAsync(entity),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Deliveries.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
