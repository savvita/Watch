using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public CategoryController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<KeyValuePair<Category, int>>>> Get()
        {
            var watches = await _context.Watches.GetAsync();
            var res = (await _context.Categories.GetAsync())
                .Select(x => new KeyValuePair<Category, int>(x, watches.Count(w => w.Category != null && w.Category.Id == x.Id))).ToList();
            return new Result<List<KeyValuePair<Category, int>>>
            {
                Value = res,
                Hits = res.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Category?>> Get(int id)
        {
            var res = await _context.Categories.GetAsync(id);
            return new Result<Category?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Category>> Create([FromBody] Category category)
        {
            var res = await _context.Categories.CreateAsync(category);
            return new Result<Category>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Category?>> Update([FromBody] Category category)
        {
            return new Result<Category?>
            {
                Value = await _context.Categories.UpdateAsync(category),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            var res = await _context.Categories.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
