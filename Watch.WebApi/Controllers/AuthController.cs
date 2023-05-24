using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Exceptions;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.Email;
using Watch.WebApi.Helpers;
using Watch.WebApi.Message;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;
        private readonly IVerification _verification;

        public AuthController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IVerification verification)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
            _userManager = userManager;
            _verification = verification;
        }

        [HttpPost("")]
        public async Task<Result<User>> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.CheckCredentialsAsync(model);

            if (user != null)
            {
                if(!user.IsActive)
                {
                    throw new InactiveUserException(user.Id);
                }

                var roles = (await _context.Users.GetRolesAsync(user)).ToList();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("IsActive", user.IsActive.ToString())
                };

                roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<User>()
                {
                    Token = token,
                    Value = new User(user)
                };
            }

            throw new InvalidCredentialsException();
        }

        [HttpPost("user")]
        public async Task<Result<User?>> Register([FromBody] RegisterModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User });

            if (user != null)
            {
                var u = await _userManager.FindByIdAsync(user.Id);
                await _verification.SendConfirmationEmailAsync(this, _configuration, u);
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim("IsActive", true.ToString())
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<User?>()
            {
                Token = token,
                Value = user
            };
        }

        [HttpGet("confirmation")]
        public async Task<Result<IResult>> ConfirmEmail()
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var user = await _userManager.FindByNameAsync(User.Identity!.Name);

            if (user != null)
            {
                var u = await _userManager.FindByIdAsync(user.Id);
                await _verification.SendConfirmationEmailAsync(this, _configuration, u);
            }

            return new Result<IResult>()
            {
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration),
                Value = Results.Ok()
            };
        }

        [HttpGet("confirmEmail")]
        public async Task<RedirectResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectPermanent(_configuration["Domain"] + "user/emailconfirmation/error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectPermanent(_configuration["Domain"] + "user/emailconfirmation/error");
            }

            var result = _verification.CheckEmailConfirmation(user, Verification.VerificationTypes.ConfirmEmail, code);
            if (result == true)
            {
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();
                return Redirect(_configuration["Domain"] + "user/emailconfirmation/success");
            }
            else
            {
                return RedirectPermanent(_configuration["Domain"] + "user/emailconfirmation/error");
            }
        }

        [HttpPost("reset")]
        public async Task<IResult> ResetPassword([FromBody]ForgotPassword model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                await _verification.SendResetPasswordEmailAsync(this, _configuration, _userManager, model.UserName);
            }

            return Results.Ok();
        }

        [HttpGet("reset")]
        public RedirectResult ResetPassword(string userId, string code)
        {
            return RedirectPermanent(_configuration["Domain"] + $"reset/{userId}/{code}");
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<Result<bool>> ChangePassword([FromBody] ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity != null)
                {
                    var username = User.Identity.Name;
                    var user = await _userManager.FindByNameAsync(username);
                    if (user != null)
                    {
                        IdentityResult result =
                            await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return new Result<bool>
                            {
                                Value = true,
                                Hits = 1,
                                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
                            };
                        }
                        else
                        {
                            return new Result<bool>
                            {
                                Value = false,
                                Hits = 0,
                                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
                            };
                        }
                    }
                }
            }

            return new Result<bool>
            {
                Value = false,
                Hits = 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };

        }


        [HttpPost("resetPassword")]
        public async Task<IResult> ResetPassword([FromBody]ResetPassword model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return Results.BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest();
            }
        }


        [HttpPost("manager")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<User?>> RegisterManager([FromBody] RegisterModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Manager });

            if (user != null)
            {
                var u = await _userManager.FindByIdAsync(user.Id);
                await _verification.SendConfirmationEmailAsync(this, _configuration, u);
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager),
                    new Claim("IsActive", true.ToString())
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<User?>()
            {
                Token = token,
                Value = user
            };
        }

        [HttpPost("admin")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<User?>> RegisterAdmin([FromBody] RegisterModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Admin });

            if (user != null)
            {
                var u = await _userManager.FindByIdAsync(user.Id);
                await _verification.SendConfirmationEmailAsync(this, _configuration, u);
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Manager),
                    new Claim(ClaimTypes.Role, UserRoles.Admin),
                    new Claim("IsActive", true.ToString())
                };

            var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

            return new Result<User?>()
            {
                Token = token,
                Value = user
            };
        }

    }
}
