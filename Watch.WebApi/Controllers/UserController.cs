using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess.UI.Roles;
using Watch.Domain.Models;
using Watch.WebApi.Exceptions;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = UserRoles.Admin)]

    public class UserController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;

        public UserController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context);
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("")]
        public async Task<Result<List<UserDisplay>>> Get()
        {
            var users = (await _context.Users.GetAsync()).Select(async u => 
            {
                var user = new UserDisplay()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsManager = await _userManager.IsInRoleAsync((UserModel)u, UserRoles.Manager),
                    IsAdmin = await _userManager.IsInRoleAsync((UserModel)u, UserRoles.Admin)
                };

                return user;
            }).Select(u => u.Result).ToList();



            return new Result<List<UserDisplay>>
            {
                Value = users,
                Hits = users.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }


        [HttpPut("")]
        public async Task<Result<UserDisplay?>> Update([FromBody] UserDisplay user)
        {
            var model = await _userManager.FindByIdAsync(user.Id);

            if(model == null)
            {
                throw new UserNotFoundException(user.UserName);
            }

            //if(await _userManager.FindByNameAsync(user.UserName) != null)
            //{
            //    throw new ConflictException();
            //}

            model.UserName = user.UserName;
            model.Email = user.Email;

            await _context.SaveChangesAsync();

            if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }


            if (user.IsAdmin)
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _userManager.AddToRoleAsync(model, UserRoles.Admin);
            }
            else
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _userManager.RemoveFromRoleAsync(model, UserRoles.Admin);
            }

            if (user.IsManager)
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.Manager))
                    await _userManager.AddToRoleAsync(model, UserRoles.Manager);
            }
            else
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.Manager))
                    await _userManager.RemoveFromRoleAsync(model, UserRoles.Manager);
            }

            return new Result<UserDisplay?>
            {
                Value = user,
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("{id}")]
        public async Task<Result<bool>> Delete(string id)
        {
            var model = await _userManager.FindByIdAsync(id);

            if (model == null)
            {
                throw new UserNotFoundException(id);
            }

            await _userManager.DeleteAsync(model);
            return new Result<bool>
            {
                Value = true,
                Hits = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
