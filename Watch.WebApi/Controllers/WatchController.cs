using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/watches")]
    public class WatchController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public WatchController(WatchDbContext context, IConfiguration configuration, IMemoryCache memoryCache, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [HttpGet("")]
        public async Task<Result<List<Watch.DataAccess.UI.Models.Watch>>> Get()
        {
            if(!_memoryCache.TryGetValue<List<Watch.DataAccess.UI.Models.Watch>>("watches", out List<Watch.DataAccess.UI.Models.Watch>? watches))
            {
                watches = (await _context.Watches.GetAsync()).ToList();

                _memoryCache.Set<List<Watch.DataAccess.UI.Models.Watch>>("watches", watches);
            }

            return new Result<List<Watch.DataAccess.UI.Models.Watch>>
            {
                Value = watches,
                Hits = watches == null ? 0 : watches.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        //TODO delete comments

        //[HttpGet("page/{page:int}")]
        //public async Task<Result<List<Watch.DataAccess.UI.Models.Watch>>> Get(int page,
        //                                                [FromQuery] int perPage,
        //                                                [FromQuery] string? model,
        //                                                [FromQuery] List<int>? categoryIds = null,
        //                                                [FromQuery] List<int>? producerIds = null,
        //                                                [FromQuery] decimal? minPrice = null,
        //                                                [FromQuery] decimal? maxPrice = null,
        //                                                [FromQuery] bool? onSale = null,
        //                                                [FromQuery] bool? isPopular = null)
        //{ 
        //    var watches = (await _context.Watches.GetAsync(model, categoryIds!.Count > 0 ? categoryIds : null, producerIds!.Count > 0 ? producerIds : null, minPrice, maxPrice, onSale, isPopular)).ToList();

        //    return new Result<List<Watch.DataAccess.UI.Models.Watch>>
        //    {
        //        Value = watches.Skip((page - 1) * perPage).Take(perPage).ToList(),
        //        Hits = watches.Count(),
        //        Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
        //    };
        //}

        [HttpGet("page/{page:int}")]
        public async Task<Result<List<Watch.DataAccess.UI.Models.Watch>>> Get(int page,
                                                        [FromQuery] int perPage,
                                                        [FromQuery] string? model = null,
                                                        [FromQuery] List<int?>? brandIds = null,
                                                        [FromQuery] List<int?>? collectionIds = null,
                                                        [FromQuery] List<int?>? styleIds = null,
                                                        [FromQuery] List<int?>? movementTypeIds = null,
                                                        [FromQuery] List<int?>? glassTypeIds = null,
                                                        [FromQuery] List<int?>? caseShapeIds = null,
                                                        [FromQuery] List<int?>? caseMaterialIds = null,
                                                        [FromQuery] List<int?>? strapTypeIds = null,
                                                        [FromQuery] List<int?>? caseColorIds = null,
                                                        [FromQuery] List<int?>? strapColorIds = null,
                                                        [FromQuery] List<int?>? dialColorIds = null,
                                                        [FromQuery] List<int?>? waterResistanceIds = null,
                                                        [FromQuery] List<int?>? incrustationTypeIds = null,
                                                        [FromQuery] List<int?>? dialTypeIds = null,
                                                        [FromQuery] List<int?>? genderIds = null,
                                                        [FromQuery] decimal? minPrice = null,
                                                        [FromQuery] decimal? maxPrice = null,
                                                        [FromQuery] List<bool>? onSale = null,
                                                        [FromQuery] List<bool>? isTop = null)
        {
            var filters = new WatchFilter
            {
                BrandId = brandIds ?? new List<int?>(),
                CaseColorId = caseColorIds ?? new List<int?>(),
                CaseMaterialId = caseMaterialIds ?? new List<int?>(),
                CaseShapeId = caseShapeIds ?? new List<int?>(),
                CollectionId = collectionIds ?? new List<int?>(),
                DialColorId = dialColorIds ?? new List<int?>(),
                DialTypeId = dialTypeIds ?? new List<int?>(),
                GenderId = genderIds ?? new List<int?>(),
                GlassTypeId = glassTypeIds ?? new List<int?>(),
                IncrustationTypeId = incrustationTypeIds ?? new List<int?>(),
                IsTop = isTop ?? new List<bool>(),
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                Model = model ?? String.Empty,
                MovementTypeId = movementTypeIds ?? new List<int?>(),
                OnSale = onSale ?? new List<bool>(),
                StrapColorId = strapColorIds ?? new List<int?>(),
                StrapTypeId = strapTypeIds ?? new List<int?>(),
                StyleId = styleIds ?? new List<int?>(),
                WaterResistanceId = waterResistanceIds ?? new List<int?>()
            };

            var watches = await _context.Watches.GetAsync(page, perPage, filters);

            return new Result<List<Watch.DataAccess.UI.Models.Watch>>
            {
                Value = watches.Value != null ? watches.Value.ToList() : new List<DataAccess.UI.Models.Watch>(),
                Hits = watches.Hits,
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
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Watch.DataAccess.UI.Models.Watch>> Create([FromBody] Watch.DataAccess.UI.Models.Watch watch)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var res = await _context.Watches.CreateAsync(watch);

            if (res != null && _memoryCache.TryGetValue<List<Watch.DataAccess.UI.Models.Watch>>("watches", out List<Watch.DataAccess.UI.Models.Watch>? watches))
            {
                _memoryCache.Remove("watches");

                if (watches != null)
                {
                    watches.Add(res);
                    _memoryCache.Set<List<Watch.DataAccess.UI.Models.Watch>>("watches", watches);
                }
            }

            return new Result<Watch.DataAccess.UI.Models.Watch>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        //TODO Check this
        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<ConcurrencyUpdateResult>> Update([FromBody] Watch.DataAccess.UI.Models.Watch watch)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            if (watch.Available == 0)
            {
                watch.OnSale = false;
            }

            var res = await _context.Watches.UpdateConcurrencyAsync(watch);

            if(res.Code == 200)
            {
                if (_memoryCache.TryGetValue<List<Watch.DataAccess.UI.Models.Watch>>("watches", out List<Watch.DataAccess.UI.Models.Watch>? watches))
                {
                    _memoryCache.Remove("watches");
                }
            }


            return new Result<ConcurrencyUpdateResult>
            {
                Value = res,
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };

            //TODO delete comments
            //var res = await _context.Watches.UpdateAsync(watch);

            //if (_memoryCache.TryGetValue<List<Watch.DataAccess.UI.Models.Watch>>("watches", out List<Watch.DataAccess.UI.Models.Watch>? watches))
            //{
            //    _memoryCache.Remove("watches");

            //    if (watches != null)
            //    {
            //        var w = watches.FirstOrDefault(x => x.Id == res.Id);
            //        if(w != null)
            //        {
            //            watches.Remove(w);
            //            watches.Add(res);
            //            _memoryCache.Set<List<Watch.DataAccess.UI.Models.Watch>>("watches", watches);
            //        }
            //    }
            //}

            //return new Result<Watch.DataAccess.UI.Models.Watch?>
            //{
            //    Value = res,
            //    Hits = 1,
            //    Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            //};
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var res = await _context.Watches.SoftDeleteAsync(id);

            if (res == true)
            {
                _memoryCache.Remove("watches");
            }

            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("restore/{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Restore(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var res = await _context.Watches.RestoreAsync(id);

            if(res == true)
            {
                _memoryCache.Remove("watches");
            }

            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
