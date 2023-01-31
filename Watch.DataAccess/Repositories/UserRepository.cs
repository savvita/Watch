using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(WatchDbContext context) : base(context)
        {
        }
    }
}
