using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Exceptions;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public OrderController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        //TODO delete comments

        //[HttpGet("")]
        //[Authorize]
        //public async Task<Result<List<Order>>> Get()
        //{
        //    await _context.Users.CheckUserAsync(User.Identity);

        //    if(!User.IsInRole(UserRoles.User)) 
        //    {
        //        throw new ForbiddenException();
        //    }

        //    var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

        //    if (username == null)
        //    {
        //        throw new InternalServerException();
        //    }

        //    var user = await _context.Users.GetByUserNameAsync(username.Value);

        //    if (user == null)
        //    {
        //        throw new UserNotFoundException(username.Value);
        //    }

        //    var orders = (await _context.Orders.GetByUserIdAsync(user.Id)).ToList();

        //    return new Result<List<Order>>
        //    {
        //        Value = orders,
        //        Hits = orders.Count,
        //        Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
        //    };
        //}


        //TODO It needs? To get absolutely all orders?
        [HttpGet("")]
        //TODO delete comments
        //[HttpGet("all")]
        [Authorize(Roles = UserRoles.Manager)]
        //public async Task<Result<List<Order>>> GetAll()
        public async Task<Result<List<Order>>> Get()
        {
            await _context.Users.CheckUserAsync(User.Identity);
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
            await _context.Users.CheckUserAsync(User.Identity);
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

            if (user.Id != userId)
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

        [HttpGet("/manager/{managerId}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Order>>> GetByManagerId(string managerId)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            var orders = (await _context.Orders.GetByManagerIdAsync(managerId)).ToList();

            return new Result<List<Order>>
            {
                Value = orders,
                Hits = orders.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<Result<Order?>> Get(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
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
            var order = await _context.Orders.GetAsync(id);

            if(order == null)
            {
                throw new InternalServerException();
            }

            if (order.Manager == null)
            {
                throw new UserNotFoundException();
            }

            if (user.Id != order.UserId && user.Id != order.Manager.Id)
            {
                throw new ForbiddenException();
            }
            return new Result<Order?>
            {
                Value = order,
                Hits = order != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPost("")]
        [Authorize]
        public async Task<Result<Order>> Create()
        {
            await _context.Users.CheckUserAsync(User.Identity);
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

            if(basket == null)
            {
                throw new BasketNotFoundException();
            }

            var res = await _context.Orders.CreateAsync(basket);
            return new Result<Order>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }


        //Close order
        [HttpPut("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Update(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);

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

            var order = await _context.Orders.GetAsync(id);

            if (order == null)
            {
                throw new OrderNotFoundException(id);
            }

            if (order.Manager == null)
            {
                throw new UserNotFoundException();
            }

            if (user.Id != order.Manager.Id)
            {
                throw new ForbiddenException();
            }

            var res = await _context.Orders.CloseOrderAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }


        //Cancel order
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
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

            var order = await _context.Orders.GetAsync(id);
            if (order == null)
            {
                throw new OrderNotFoundException(id);
            }

            if(order.Manager == null)
            {
                throw new UserNotFoundException();
            }

            if (user.Id != order.UserId && user.Id != order.Manager.Id)
            {
                throw new ForbiddenException();
            }

            var res = await _context.Orders.CancelOrderAsync(id);

            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
