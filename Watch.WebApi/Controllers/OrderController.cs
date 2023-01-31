using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess.UI.Roles;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public OrderController(WatchDbContext context, IConfiguration configuration)
        {
            _context = new DbContext(context);
            _configuration = configuration;
        }

        [HttpGet("")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<List<Order>>> Get()
        {
            return new Result<List<Order>>
            {
                Value = (await _context.Orders.GetAsync()).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<Order?>> Get(int id)
        {
            return new Result<Order?>
            {
                Value = await _context.Orders.GetAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPost("")]
        [Authorize]
        public async Task<Result<Order>> Create([FromBody] Order order)
        {
            return new Result<Order>
            {
                Value = await _context.Orders.CreateAsync(order),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<Order?>> Update([FromBody] Order order)
        {
            return new Result<Order?>
            {
                Value = await _context.Orders.UpdateAsync(order),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<bool>> Delete(int id)
        {
            return new Result<bool>
            {
                Value = await _context.Orders.DeleteAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
