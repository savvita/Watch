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
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public PaymentController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        //TODO check receiving all and only active
        [HttpGet("")]
        public async Task<Result<List<Payment>>> Get([FromQuery]bool? all)
        {
            var values = await _context.Payments.GetAsync();
            if(all == null || all == false)
            {
                values = values.Where(x => x.IsActive).ToList();
            }

            return new Result<List<Payment>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Payment?>> Get(int id)
        {
            var res = await _context.Payments.GetAsync(id);
            return new Result<Payment?>
            {
                Value = res,
                Hits = res == null ? 0 : 1,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<Payment>> Create([FromBody] Payment entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Payments.CreateAsync(entity);
            return new Result<Payment>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<Payment?>> Update([FromBody] Payment entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Payment?>
            {
                Value = await _context.Payments.UpdateAsync(entity),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Payments.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
