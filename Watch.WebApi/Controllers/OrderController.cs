using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess.UI.Roles;
using Watch.WebApi.Exceptions;

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
            var orders = (await _context.Orders.GetAsync()).ToList();
            return new Result<List<Order>>
            {
                Value = orders,
                Hits = orders.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("/user/{userId}")]
        [Authorize]
        public async Task<Result<List<Order>>> GetByUserId(string userId)
        {
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            if (user.Id != userId && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }

            var orders = (await _context.Orders.GetByUserIdAsync(userId)).ToList();

            return new Result<List<Order>>
            {
                Value = orders,
                Hits = orders.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Order?>> Get(int id)
        {
            var res = await _context.Orders.GetAsync(id);
            return new Result<Order?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPost("")]
        [Authorize]
        public async Task<Result<Order>> Create([FromBody] Basket basket)
        {
            var res = await _context.Orders.CreateAsync(basket);
            return new Result<Order>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Order?>> Update([FromBody] Order order)
        {
            return new Result<Order?>
            {
                Value = await _context.Orders.UpdateAsync(order),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            var res = await _context.Orders.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
