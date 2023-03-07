using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }

        public async Task<UserModel?> GetAsync(string id)
        {
            return await Task.FromResult<UserModel?>(_db.Users.FirstOrDefault(u => u.Id.Equals(id)));
        }

        public async Task<UserModel?> GetByUserNameAsync(string username)
        {
            return await Task.FromResult<UserModel?>(_db.Users.FirstOrDefault(u => u.UserName.Equals(username)));
        }

        public async Task<bool> SetActivityAsync(string id, bool activity)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = activity;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
