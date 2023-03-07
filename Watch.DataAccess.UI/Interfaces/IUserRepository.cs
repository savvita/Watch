using System.Security.Principal;

namespace Watch.DataAccess.UI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAsync(string username);
        Task<User?> GetAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> RestoreAsync(string id);

        Task<User?> CreateAsync(RegisterModel entity, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetRolesAsync(UserModel entity);
        Task<UserModel?> CheckCredentialsAsync(LoginModel model);
        Task<bool> CheckUserAsync(IIdentity? identity);
    }
}
