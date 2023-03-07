using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task<UserModel?> GetByUserNameAsync(string username);
        Task<UserModel?> GetAsync(string id);
        Task<bool> SetActivityAsync(string id, bool activity);
    }
}
