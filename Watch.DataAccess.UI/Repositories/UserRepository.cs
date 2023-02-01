using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public UserRepository(WatchDbContext context)
        {
            _db = new UnitOfWorks.UnitOfWorks(context);
        }
        public async Task<User?> CreateAsync(User entity)
        {
            var model = await _db.Users.CreateAsync((UserModel)entity);

            return model != null ? new User(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Users.DeleteAsync(id);
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return (await _db.Users.GetAsync()).Select(model => new User(model));
        }

        public async Task<User?> GetAsync(int id)
        {
            var model = await _db.Users.GetAsync(id);

            return model != null ? new User(model) : null;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var model = await _db.Users.UpdateAsync((UserModel)entity);

            return new User(model);
        }

    }
}
