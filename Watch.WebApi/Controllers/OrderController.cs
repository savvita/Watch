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
using Watch.WebApi.Helpers;

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

        [HttpGet("")]
        [Authorize]
        public async Task<Result<List<Order>>> Get([FromQuery]bool? isUser, [FromQuery]bool? isManager, [FromQuery]List<int>? statusses)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            List<Order> orders = (await _context.Orders.GetAsync()).ToList();

            if (isManager != null && isManager == true)
            {
                if (!User.IsInRole(UserRoles.Manager))
                {
                    throw new ForbiddenException();
                }

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

                orders = orders.Where(item => item.Manager != null && item.Manager.Id == user.Id).ToList();
            }

            if (isUser != null && isUser == true)
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

                orders = orders.Where(item => item.UserId == user.Id).ToList();
            }

            if(statusses != null && statusses.Count > 0)
            {
                orders = orders.Where(item => item.Status != null && statusses.Contains(item.Status.Id)).ToList();
            }

            return new Result<List<Order>>
            {
                Value = orders,
                Hits = orders.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("user/{id}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Order>>> Get(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var res = (await _context.Orders.GetByUserIdAsync(id)).ToList();
           

            return new Result<List<Order>>
            {
                Value = res,
                Hits = res.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("sales")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Watch.DataAccess.UI.Models.Order>>> Get(
                                                        [FromQuery] int? watchId = null,
                                                        [FromQuery] int? brandId = null,
                                                        [FromQuery] int? collectionId = null,
                                                        [FromQuery] int? styleId = null,
                                                        [FromQuery] int? movementTypeId = null,
                                                        [FromQuery] int? glassTypeId = null,
                                                        [FromQuery] int? countryId = null,
                                                        [FromQuery] int? caseShapeId = null,
                                                        [FromQuery] int? caseMaterialId = null,
                                                        [FromQuery] int? strapTypeId = null,
                                                        [FromQuery] int? caseColorId = null,
                                                        [FromQuery] int? strapColorId = null,
                                                        [FromQuery] int? dialColorId = null,
                                                        [FromQuery] int? functionId = null,
                                                        [FromQuery] int? waterResistanceId = null,
                                                        [FromQuery] int? incrustationTypeId = null,
                                                        [FromQuery] int? dialTypeId = null,
                                                        [FromQuery] int? genderId = null)
        {
            var filters = new OrderFilter
            {
                WatchId = watchId,
                BrandId = brandId,
                CollectionId = collectionId,
                DialTypeId = dialTypeId,
                DialColorId = dialColorId,
                CountryId = countryId,
                StrapColorId = strapColorId,
                StrapTypeId = strapTypeId,
                GenderId = genderId,
                CaseColorId = caseColorId,
                CaseMaterialId = caseMaterialId,
                CaseShapeId = caseShapeId,
                GlassTypeId = glassTypeId,
                IncrustationTypeId = incrustationTypeId,
                MovementTypeId = movementTypeId,
                StyleId = styleId,
                WaterResistanceId = waterResistanceId,
                FunctionId = functionId
            };

            var orders = await _context.Orders.GetAsync(filters);

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

            if (user.Id != order.UserId && order.Manager != null && user.Id != order.Manager.Id)
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
        public async Task<Result<Order>> Create([FromBody]OrderAdditionalInfo info)
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

            var res = await _context.Orders.CreateAsync(basket, info);
            return new Result<Order>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }


        [HttpPut("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Update(int id, [FromQuery] int? statusId, [FromQuery] string? en)
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

            if (order.Manager == null && order.Status != null && order.Status.Id != 1)
            {
                throw new UserNotFoundException();
            }

            else if (order.Manager != null && user.Id != order.Manager.Id)
            {
                throw new ForbiddenException();
            }

            bool res = true;

            if (statusId != null) 
            {
                switch(statusId)
                {
                    case 2:
                        res = await _context.Orders.AcceptOrderAsync(id, user.Id); 
                        break;

                    case 3:
                        res = await _context.Orders.CloseOrderAsync(id);
                        break;

                    case 4:
                        res = await _context.Orders.CancelOrderAsync(id);
                        break;

                    case 5:
                    case 6:
                        res = await _context.Orders.SetOrderStatusAsync(id, (int)statusId);
                        break;

                    default:
                        res = false; 
                        break;
                }
            }

            if(en != null)
            {
                order.EN = en;
                await _context.Orders.UpdateAsync(order);
            }

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


            if (user.Id != order.UserId && order.Manager != null && user.Id != order.Manager.Id)
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
