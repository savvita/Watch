﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/genders")]
    public class GenderController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public GenderController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Gender>>> Get()
        {
            var values = await _context.Genders.GetAsync();

            return new Result<List<Gender>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Gender?>> Get(int id)
        {
            var res = await _context.Genders.GetAsync(id);
            return new Result<Gender?>
            {
                Value = res,
                Hits = await _context.Watches.Count.Gender(id),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Gender>> Create([FromBody] Gender entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Genders.CreateAsync(entity);
            return new Result<Gender>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Gender?>> Update([FromBody] Gender entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Gender?>
            {
                Value = await _context.Genders.UpdateAsync(entity),
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Genders.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
