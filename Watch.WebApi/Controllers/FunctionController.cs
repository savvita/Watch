﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/functions")]
    public class FunctionController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public FunctionController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Function>>> Get()
        {
            var values = await _context.Functions.GetAsync();

            return new Result<List<Function>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<Result<Function?>> Get(int id)
        {
            var res = await _context.Functions.GetAsync(id);
            return new Result<Function?>
            {
                Value = res,
                Hits = await _context.Watches.Count.Function(id),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Function>> Create([FromBody] Function entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Functions.CreateAsync(entity);
            return new Result<Function>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Function?>> Update([FromBody] Function entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Function?>
            {
                Value = await _context.Functions.UpdateAsync(entity),
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Functions.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("sales")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Sale>>> GetSales()
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Functions.GetSalesAsync();

            return new Result<List<Sale>>
            {
                Value = res,
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
