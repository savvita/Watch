using Watch.DataAccess.UI.Models;

namespace Watch.DataAccess.UI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAsync(string username);
    }
}
