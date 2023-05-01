using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch.DataAccess.UI.Models;
using Watch.DataAccess;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.DataAccess.UI;
using System.Security.Claims;
using Watch.DataAccess.UI.Exceptions;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public ReviewController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("all")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<Review>>> Get([FromQuery]bool? check = null)
        {
            var res = await _context.Reviews.GetAsync();
            if(check != null)
            {
                res = res.Where(x => x.Checked == check);
            }

            return new Result<List<Review>>
            {
                Value = res.ToList(),
                Hits = res.Count(),
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<Result<List<Review>>> GetByUser([FromQuery]string? id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            if (id == null && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }

            List<Review> res = new List<Review>();

            if(id == null)
            {
                res = await _context.Reviews.GetByUserIdAsync(user.Id);
            }
            else if(User.IsInRole(UserRoles.Manager))
            {
                res = await _context.Reviews.GetByUserIdAsync(id);
            }

            return new Result<List<Review>>
            {
                Value = res,
                Hits = res.Count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("watch/{watchId:int}")]
        public async Task<Result<List<Review>>> Get(int watchId)
        {
            var values = await _context.Reviews.GetByWatchIdAsync(watchId);

            return new Result<List<Review>>
            {
                Value = values.ToList(),
                Hits = values.Count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("")]
        public async Task<Result<Review?>> GetById([FromQuery]int id)
        {
            var res = await _context.Reviews.GetAsync(id);

            return new Result<Review?>
            {
                Value = res,
                Hits = res != null? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPost("{watchId:int}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<Result<Review>> Create(int watchId, [FromBody] string text)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            var entity = new Review
            {
                Date = DateTime.Now,
                Text = text,
                UserId = user.Id,
                UserName = username.Value,
                WatchId = watchId
            };

            var res = await _context.Reviews.CreateAsync(entity);
            return new Result<Review>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpPut("")]
        [Authorize]
        public async Task<Result<Review?>> Update([FromBody] Review entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            if (user.Id != entity.UserId && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }

            return new Result<Review?>
            {
                Value = await _context.Reviews.UpdateAsync(entity),
                Hits = 1,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<Result<bool>> Delete(int id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }

            var entity = await _context.Reviews.GetAsync(id);

            if(entity == null)
            {
                throw new ReviewNotFoundException(id);
            }

            if (user.Id != entity.UserId && !User.IsInRole(UserRoles.Manager))
            {
                throw new ForbiddenException();
            }

            var res = await _context.Reviews.DeleteAsync(id);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
