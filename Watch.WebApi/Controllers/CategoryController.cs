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
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public CategoryController(WatchDbContext context, IConfiguration configuration)
        {
            _context = new DbContext(context);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<KeyValuePair<Category, int>>>> Get()
        {
            var watches = await _context.Watches.GetAsync();
            return new Result<List<KeyValuePair<Category, int>>>
            {
                Value = (await _context.Categories.GetAsync())
                .Select(x => new KeyValuePair<Category, int>(x, watches.Count(w => w.Category != null && w.Category.Id == x.Id))).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Category?>> Get(int id)
        {
            return new Result<Category?>
            {
                Value = await _context.Categories.GetAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPost("")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<Category>> Create([FromBody] Category category)
        {
            return new Result<Category>
            {
                Value = await _context.Categories.CreateAsync(category),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<Category?>> Update([FromBody] Category category)
        {
            return new Result<Category?>
            {
                Value = await _context.Categories.UpdateAsync(category),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<bool>> Delete(int id)
        {
            return new Result<bool>
            {
                Value = await _context.Categories.DeleteAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
