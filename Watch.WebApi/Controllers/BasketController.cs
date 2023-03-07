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
    [Route("api/baskets")]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public BasketController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<Basket>> Get()
        {
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (usernameClaim == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(usernameClaim.Value);

            if (user == null)
            {
                throw new UserNotFoundException(usernameClaim.Value);
            }

            var basket = await _context.Baskets.GetByUserIdAsync(user.Id);

            if (basket == null)
            {
                basket = await _context.Baskets.CreateAsync(new Basket()
                {
                    UserId = user.Id
                });

                if (basket == null)
                {
                    throw new InternalServerException();
                }
            }

            return new Result<Basket>
            {
                Value = basket,
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }


        //TODO delete comments

        //[HttpPost("")]
        //public async Task<Result<Basket>> Create([FromBody] Basket basket)
        //{
        //    var res = await _context.Baskets.CreateAsync(basket);
        //    return new Result<Basket>
        //    {
        //        Value = res,
        //        Hits = res != null ? 1 : 0,
        //        Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
        //    };
        //}

        //[HttpPost("")]
        //public async Task<Result<Basket>> Create([FromBody] BasketDetail detail)
        //{
        //    var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

        //    if (usernameClaim == null)
        //    {
        //        throw new InternalServerException();
        //    }

        //    var user = await _context.Users.GetByUserNameAsync(usernameClaim.Value);

        //    if (user == null)
        //    {
        //        throw new UserNotFoundException(usernameClaim.Value);
        //    }

        //    var basket = await _context.Baskets.GetByUserIdAsync(user.Id);

        //    if(basket == null)
        //    {
        //        basket = await _context.Baskets.CreateAsync(new Basket()
        //        {
        //            UserId = user.Id
        //        });

        //        if(basket == null)
        //        {
        //            throw new InternalServerException();
        //        }
        //    }

        //    var d = basket.Details.FirstOrDefault(x => x.WatchId == detail.WatchId);

        //    if(d == null)
        //    {
        //        detail.BasketId = basket.Id;
        //        basket.Details.Add(detail);
        //    }
        //    else
        //    {
        //        d.Count += detail.Count;
        //    }         

        //    var res = await _context.Baskets.UpdateAsync(basket);
        //    return new Result<Basket>
        //    {
        //        Value = res,
        //        Hits = res != null ? 1 : 0,
        //        Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
        //    };
        //}

        //[HttpPut("")]
        //public async Task<Result<Basket?>> Update([FromBody] Basket basket)
        //{
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

        //    var res = await _context.Baskets.GetByUserIdAsync(user.Id);

        //    if (res != null && res.UserId != user.Id && !User.IsInRole(UserRoles.Manager))
        //    {
        //        throw new ForbiddenException();
        //    }
        //    return new Result<Basket?>
        //    {
        //        Value = await _context.Baskets.UpdateAsync(basket),
        //        Hits = 1,
        //        Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
        //    };
        //}

        [HttpPut("")]
        public async Task<Result<Basket?>> Update([FromBody] List<BasketDetail> details)
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

            if (basket == null)
            {
                throw new InternalServerException();
            }

            basket.Details = details;

            return new Result<Basket?>
            {
                Value = await _context.Baskets.UpdateAsync(basket),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("")]
        public async Task<Result<bool>> Clear() 
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

            if (basket == null)
            {
                throw new InternalServerException();
            }

            var res = await _context.Baskets.DeleteAsync(basket.Id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        //TODO delete comments

        //[HttpDelete("")]
        //public async Task<Result<bool>> Delete()
        //{
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

        //    var basket = await _context.Baskets.GetByUserIdAsync(user.Id);

        //    if (basket == null)
        //    {
        //        throw new InternalServerException();
        //    }

        //    var res = await _context.Baskets.DeleteAsync(basket.Id);
        //    return new Result<bool>
        //    {
        //        Value = res,
        //        Hits = res == true ? 1 : 0,
        //        Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
        //    };
        //}

        //[HttpDelete("{id:int}")]
        //[Authorize(Roles = UserRoles.Manager)]
        //public async Task<Result<bool>> Delete(int id)
        //{
        //    var res = await _context.Baskets.DeleteAsync(id);
        //    return new Result<bool>
        //    {
        //        Value = res,
        //        Hits = res == true ? 1 : 0,
        //        Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
        //    };
        //}
    }
}
