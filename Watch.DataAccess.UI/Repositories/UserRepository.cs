using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using Watch.DataAccess.UI.Exceptions;
using Watch.DataAccess.UI.Interfaces;
using Watch.Domain.Roles;

namespace Watch.DataAccess.UI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public UserRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }
        public async Task<User?> CreateAsync(User entity)
        {
            var user = await _db.UserManager.FindByNameAsync(entity.UserName);

            if (user != null)
            {
                throw new ConflictException();
            }

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
                FirstName = entity.FirstName,
                SecondName = entity.SecondName,
                LastName = entity.LastName,
                IsActive = true,
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

            return await _db.Users.SetActivityAsync(id, false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
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
            return await Task.FromResult<User?>(null);
        }

        public async Task<User?> GetAsync(string id)
        {
            var model = await _db.Users.GetAsync(id);

            if (model == null)
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
            var user = await _db.UserManager.FindByNameAsync(entity.UserName);

            if(user != null && model.Id != user.Id)
            {
                throw new ConflictException();
            }

            model.UserName = entity.UserName;
            model.Email = entity.Email;
            model.FirstName = entity.FirstName;
            model.SecondName = entity.SecondName;
            model.LastName = entity.LastName;
            model.IsActive = entity.IsActive;
            model.IsActive = entity.IsActive;

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

        public async Task<bool> RestoreAsync(string id)
        {
            var model = await _db.UserManager.FindByIdAsync(id);

            if (model == null)
            {
                throw new UserNotFoundException(id);
            }

            if (model.IsActive == true)
            {
                return false;
            }

            await _db.Users.SetActivityAsync(id, true);

            return true;
        }

        public async Task<bool> CheckUserAsync(IIdentity? identity)
        {
            if(identity == null || identity.Name == null)
            {
                throw new ArgumentNullException();
            }

            var user = await _db.UserManager.FindByNameAsync(identity.Name);

            if (user == null)
            {
                throw new UserNotFoundException(identity.Name);
            }

            if(!user.IsActive)
            {
                throw new InactiveUserException(user.Id);
            }

            return true;
        }
    }
}
