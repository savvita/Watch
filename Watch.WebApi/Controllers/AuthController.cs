using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess.UI.Roles;
using Watch.Domain.Models;
using Watch.WebApi.Exceptions;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;
        public AuthController(IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("")]
        public async Task<Result<string>> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToList();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<string>()
                {
                    Token = token,
                    Value = token
                };
            }

            throw new InvalidCredentialsException();
        }

        [HttpPost("user")]
        public async Task<Result<string>> Register([FromBody] RegisterModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                throw new ConflictException();
            }

            UserModel iUser = new UserModel()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(iUser, model.Password);

            if (!result.Succeeded)
            {
                throw new AuthorizationException();
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(iUser, UserRoles.User);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User)
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<string>()
            {
                Token = token,
                Value = token
            };
        }

        [HttpPost("manager")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<string>> RegisterManager([FromBody] RegisterModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                throw new ConflictException();
            }

            UserModel iUser = new UserModel()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(iUser, model.Password);

            if (!result.Succeeded)
            {
                throw new AuthorizationException();
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(iUser, UserRoles.User);

            if (await _roleManager.RoleExistsAsync(UserRoles.Manager))
                await _userManager.AddToRoleAsync(iUser, UserRoles.Manager);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager)
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<string>()
            {
                Token = token,
                Value = token
            };
        }

        [HttpPost("admin")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<string>> RegisterAdmin([FromBody] RegisterModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                throw new ConflictException();
            }

            UserModel iUser = new UserModel()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(iUser, model.Password);

            if (!result.Succeeded)
            {
                throw new AuthorizationException();
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(iUser, UserRoles.User);

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(iUser, UserRoles.Admin);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager),
                    new Claim(ClaimTypes.Role, UserRoles.Admin)
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<string>()
            {
                Token = token,
                Value = token
            };
        }

    }
}
