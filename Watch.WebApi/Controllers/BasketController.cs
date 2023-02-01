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
    [Route("api/baskets")]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public BasketController(WatchDbContext context, IConfiguration configuration)
        {
            _context = new DbContext(context);
            _configuration = configuration;
        }

        [HttpGet("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Basket>>> Get()
        {
            var baskets = (await _context.Baskets.GetAsync()).ToList();
            return new Result<List<Basket>>
            {
                Value = baskets,
                Hits = baskets.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("{userId}")]
        public async Task<Result<Basket?>> Get(string userId)
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

            var res = await _context.Baskets.GetByUserIdAsync(userId);

            if(res != null && res.UserId != user.Id && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }

            return new Result<Basket?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPost("")]
        public async Task<Result<Basket>> Create([FromBody] Basket basket)
        {
            var res = await _context.Baskets.CreateAsync(basket);
            return new Result<Basket>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        public async Task<Result<Basket?>> Update([FromBody] Basket basket)
        {
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            var res = await _context.Baskets.GetByUserIdAsync(user.Id);

            if (res != null && res.UserId != user.Id && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }
            return new Result<Basket?>
            {
                Value = await _context.Baskets.UpdateAsync(basket),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<Result<bool>> Delete(int id)
        {
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            var basket = await _context.Baskets.GetByUserIdAsync(user.Id);

            if (basket != null && basket.UserId != user.Id && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }

            var res = await _context.Baskets.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
