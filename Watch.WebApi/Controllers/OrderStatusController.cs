using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/orderstatusses")]
    public class OrderStatusController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public OrderStatusController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<OrderStatus>>> Get()
        {
            var values = (await _context.OrderStatuses.GetAsync()).ToList();
            return new Result<List<OrderStatus>>
            {
                Value = values,
                Hits = values.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }
    }
}
