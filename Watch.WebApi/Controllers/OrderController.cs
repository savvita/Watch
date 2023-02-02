﻿using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<Result<List<Order>>> Get()
        {
            var orders = (await _context.Orders.GetAsync()).ToList();

            if(User.IsInRole(UserRoles.User))
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

                orders = orders.Where(o => o.UserId == user.Id).ToList();
            }

            return new Result<List<Order>>
            {
                Value = orders,
                Hits = orders.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("all")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Order>>> GetAll()
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
        [Authorize]
        public async Task<Result<Order?>> Get(int id)
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
            var order = await _context.Orders.GetAsync(id);

            if(order == null)
            {
                throw new InternalServerException();
            }

            if (user.Id != order.UserId && !User.IsInRole(UserRoles.Manager))
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
                throw new InternalServerException();
            }

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
        [Authorize]
        public async Task<Result<Order?>> Update([FromBody] Order order)
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

            if (user.Id != order.UserId && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }
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
            var res = await _context.Orders.CloseOrderAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
