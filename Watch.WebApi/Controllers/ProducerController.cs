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
    [Route("api/producers")]
    public class ProducerController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public ProducerController(WatchDbContext context, IConfiguration configuration)
        {
            _context = new DbContext(context);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<KeyValuePair<Producer, int>>>> Get()
        {
            var watches = await _context.Watches.GetAsync();
            var producers = (await _context.Producers.GetAsync())
                .Select(x => new KeyValuePair<Producer, int>(x, watches.Count(w => w.Producer != null && w.Producer.Id == x.Id))).ToList();
            return new Result<List<KeyValuePair<Producer, int>>>
            {
                Value = producers,
                Hits = producers.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Producer?>> Get(int id)
        {
            var res = await _context.Producers.GetAsync(id);
            return new Result<Producer?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Producer>> Create([FromBody] Producer producer)
        {
            var res = await _context.Producers.CreateAsync(producer);
            return new Result<Producer>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Producer?>> Update([FromBody] Producer producer)
        {
            return new Result<Producer?>
            {
                Value = await _context.Producers.UpdateAsync(producer),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            var res = await _context.Producers.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
