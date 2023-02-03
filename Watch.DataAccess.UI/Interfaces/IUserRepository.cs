using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAsync(string username);
        Task<bool> DeleteAsync(string id);

        Task<User?> CreateAsync(RegisterModel entity, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetRolesAsync(UserModel entity);
        Task<UserModel?> CheckCredentialsAsync(LoginModel model);
    }
}
