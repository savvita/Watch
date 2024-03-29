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
    [Route("api/materials")]
    public class MaterialController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public MaterialController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Material>>> Get()
        {
            var values = await _context.Materials.GetAsync();

            return new Result<List<Material>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("{type}/{id:int}")]
        public async Task<Result<Material?>> Get(string type, int id)
        {
            var res = await _context.Materials.GetAsync(id);
            int count = 0;
            switch(type)
            {
                case "case":
                    count = await _context.Watches.Count.CaseMaterial(id);
                    break;
            }
            return new Result<Material?>
            {
                Value = res,
                Hits = count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Material>> Create([FromBody] Material entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Materials.CreateAsync(entity);
            return new Result<Material>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<Material?>> Update([FromBody] Material entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            return new Result<Material?>
            {
                Value = await _context.Materials.UpdateAsync(entity),
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Materials.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("sales/{type}")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Sale>>> GetSales(string type)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var res = await _context.Materials.GetSalesAsync(type);

            return new Result<List<Sale>>
            {
                Value = res,
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
