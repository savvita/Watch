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
    [Route("api/watches")]
    public class WatchController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public WatchController(WatchDbContext context, IConfiguration configuration)
        {
            _context = new DbContext(context);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Watch.DataAccess.UI.Models.Watch>>> Get()
        {
            return new Result<List<Watch.DataAccess.UI.Models.Watch>>
            {
                Value = (await _context.Watches.GetAsync()).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("page/{page:int}")]
        public async Task<Result<List<Watch.DataAccess.UI.Models.Watch>>> Get(int page,
                                                        [FromQuery] int perPage,
                                                        [FromQuery] string? model,
                                                        [FromQuery] List<int>? categoryIds = null,
                                                        [FromQuery] List<int>? producerIds = null,
                                                        [FromQuery] decimal? minPrice = null,
                                                        [FromQuery] decimal? maxPrice = null,
                                                        [FromQuery] bool? onSale = null)
        {
            return new Result<List<Watch.DataAccess.UI.Models.Watch>>
            {
                Value = (await _context.Watches.GetAsync(page, perPage, model, categoryIds!.Count > 0 ? categoryIds : null, producerIds!.Count > 0 ? producerIds : null, minPrice, maxPrice, onSale)).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch?>> Get(int id)
        {
            return new Result<Watch.DataAccess.UI.Models.Watch?>
            {
                Value = await _context.Watches.GetAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPost("")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch>> Create([FromBody] Watch.DataAccess.UI.Models.Watch watch)
        {
            return new Result<Watch.DataAccess.UI.Models.Watch>
            {
                Value = await _context.Watches.CreateAsync(watch),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch?>> Update([FromBody] Watch.DataAccess.UI.Models.Watch watch)
        {
            return new Result<Watch.DataAccess.UI.Models.Watch?>
            {
                Value = await _context.Watches.UpdateAsync(watch),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<bool>> Delete(int id)
        {
            return new Result<bool>
            {
                Value = await _context.Watches.SoftDeleteAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("restore/{id:int}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        public async Task<Result<bool>> Restore(int id)
        {
            return new Result<bool>
            {
                Value = await _context.Watches.RestoreAsync(id),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
