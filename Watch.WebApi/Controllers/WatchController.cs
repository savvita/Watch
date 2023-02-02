using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess.UI.Roles;
using Watch.WebApi.Cache;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/watches")]
    public class WatchController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        public WatchController(WatchDbContext context, IConfiguration configuration, ICacheService cacheService)
        {
            _context = new DbContext(context);
            _configuration = configuration;
            _cacheService = cacheService;
        }

        [HttpGet("")]
        public async Task<Result<List<Watch.DataAccess.UI.Models.Watch>>> Get()
        {
            var cached = await _cacheService.GetData<List<Watch.DataAccess.UI.Models.Watch>>("watches");

            if(cached == null)
            {
                var watches = (await _context.Watches.GetAsync()).ToList();
                if (watches.Count > 0)
                {
                    await _cacheService.SetData("watches", watches, DateTimeOffset.Now.AddDays(1));
                    cached = watches;
                }
            }
            return new Result<List<Watch.DataAccess.UI.Models.Watch>>
            {
                Value = cached,
                Hits = cached == null ? 0 : cached.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
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
                                                        [FromQuery] bool? onSale = null,
                                                        [FromQuery] bool? isPopular = null)
        { 
            var watches = (await _context.Watches.GetAsync(model, categoryIds!.Count > 0 ? categoryIds : null, producerIds!.Count > 0 ? producerIds : null, minPrice, maxPrice, onSale, isPopular)).ToList();

            return new Result<List<Watch.DataAccess.UI.Models.Watch>>
            {
                Value = watches.Skip((page - 1) * perPage).Take(perPage).ToList(),
                Hits = watches.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch?>> Get(int id)
        {
            var res = await _context.Watches.GetAsync(id);
            return new Result<Watch.DataAccess.UI.Models.Watch?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch>> Create([FromBody] Watch.DataAccess.UI.Models.Watch watch)
        {
            await _cacheService.RemoveData("watches");
            var res = await _context.Watches.CreateAsync(watch);
            return new Result<Watch.DataAccess.UI.Models.Watch>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch?>> Update([FromBody] Watch.DataAccess.UI.Models.Watch watch)
        {
            await _cacheService.RemoveData("watches");
            if (watch.Available == 0)
            {
                watch.OnSale = false;
            }

            return new Result<Watch.DataAccess.UI.Models.Watch?>
            {
                Value = await _context.Watches.UpdateAsync(watch),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _cacheService.RemoveData("watches");
            var res = await _context.Watches.SoftDeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("restore/{id:int}")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Restore(int id)
        {
            await _cacheService.RemoveData("watches");
            var res = await _context.Watches.RestoreAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
