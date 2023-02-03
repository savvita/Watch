using Microsoft.AspNetCore.Identity;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(WatchDbContext context) : base(context)
        {
        }

        public async Task<UserModel?> GetByUserNameAsync(string username)
        {
            return await Task.FromResult<UserModel?>(_db.Users.FirstOrDefault(u => u.UserName.Equals(username)));
        }
    }
}
