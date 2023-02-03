using Microsoft.AspNetCore.Identity;
using Watch.DataAccess.UI.Exceptions;
using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;

namespace Watch.DataAccess.UI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public UserRepository(WatchDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = new UnitOfWorks.UnitOfWorks(context, userManager, roleManager);
        }
        public async Task<User?> CreateAsync(User entity)
        {
            var model = await _db.Users.CreateAsync((UserModel)entity);

            return model != null ? new User(model) : null;
        }
        
        public async Task<User?> CreateAsync(RegisterModel entity, IEnumerable<string> roles)
        {
            var user = await _db.UserManager.FindByNameAsync(entity.UserName);

            if (user != null)
            {
                throw new ConflictException();
            }

            UserModel model = new UserModel()
            {
                UserName = entity.UserName,
                Email = entity.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _db.UserManager.CreateAsync(model, entity.Password);

            if (!result.Succeeded)
            {
                throw new AuthorizationException();
            }

            foreach(var role in roles)
            {
                await AddToRoleAsync(model, role);
            }

            return new User(model);
        }

        private async Task AddToRoleAsync(UserModel user, string role)
        {
            if (!await _db.Roles.RoleExistsAsync(role))
            {
                await _db.Roles.CreateAsync(new IdentityRole(role));
            }

            if (await _db.Roles.RoleExistsAsync(role))
                await _db.UserManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var model = await _db.UserManager.FindByIdAsync(id);

            if (model == null)
            {
                throw new UserNotFoundException(id);
            }

            await _db.UserManager.DeleteAsync(model);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return false;
        }


        public async Task<IEnumerable<User>> GetAsync()
        {
            var users = (await _db.Users.GetAsync()).Select(async u =>
            {
                var user = new User(u);
                user.IsManager = await _db.UserManager.IsInRoleAsync(u, UserRoles.Manager);
                user.IsAdmin = await _db.UserManager.IsInRoleAsync(u, UserRoles.Admin);

                return user;
            }).Select(u => u.Result).ToList();

            return users;
        }

        public async Task<User?> GetAsync(int id)
        {
            var model = await _db.Users.GetAsync(id);

            if(model == null)
            {
                return null;
            }

            var user = new User(model);
            user.IsManager = await _db.UserManager.IsInRoleAsync(model, UserRoles.Manager);
            user.IsAdmin = await _db.UserManager.IsInRoleAsync(model, UserRoles.Admin);

            return user;
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            var model = await _db.Users.GetByUserNameAsync(username);

            if (model == null)
            {
                return null;
            }

            var user = new User(model);
            user.IsManager = await _db.UserManager.IsInRoleAsync(model, UserRoles.Manager);
            user.IsAdmin = await _db.UserManager.IsInRoleAsync(model, UserRoles.Admin);

            return user;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var model = await _db.UserManager.FindByIdAsync(entity.Id);

            if (model == null)
            {
                throw new UserNotFoundException(entity.Id);
            }

            if (model.UserName != entity.UserName && await _db.UserManager.FindByNameAsync(entity.UserName) != null)
            {
                throw new ConflictException();
            }

            model.UserName = entity.UserName;
            model.Email = entity.Email;

            await _db.Users.UpdateAsync(model);

            if (!await _db.Roles.RoleExistsAsync(UserRoles.Manager))
            {
                await _db.Roles.CreateAsync(new IdentityRole(UserRoles.Manager));
            }

            if (!await _db.Roles.RoleExistsAsync(UserRoles.Admin))
            {
                await _db.Roles.CreateAsync(new IdentityRole(UserRoles.Admin));
            }


            if (entity.IsAdmin)
            {
                if (await _db.Roles.RoleExistsAsync(UserRoles.Admin))
                    await _db.UserManager.AddToRoleAsync(model, UserRoles.Admin);
            }
            else
            {
                if (await _db.Roles.RoleExistsAsync(UserRoles.Admin))
                    await _db.UserManager.RemoveFromRoleAsync(model, UserRoles.Admin);
            }

            if (entity.IsManager)
            {
                if (await _db.Roles.RoleExistsAsync(UserRoles.Manager))
                    await _db.UserManager.AddToRoleAsync(model, UserRoles.Manager);
            }
            else
            {
                if (await _db.Roles.RoleExistsAsync(UserRoles.Manager))
                    await _db.UserManager.RemoveFromRoleAsync(model, UserRoles.Manager);
            }

            return entity;
        }

        public async Task<IEnumerable<string>> GetRolesAsync(UserModel entity)
        {
            return (await _db.UserManager.GetRolesAsync(entity)).ToList();
        }

        public async Task<UserModel?> CheckCredentialsAsync(LoginModel model)
        {
            var user = await _db.UserManager.FindByNameAsync(model.UserName);

            if (user != null && await _db.UserManager.CheckPasswordAsync(user, model.Password))
            {
                return user;
            }

            return null;
        }
    }
}
