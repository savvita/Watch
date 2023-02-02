using Watch.DataAccess.UI.Models;

namespace Watch.DataAccess.UI.Interfaces
{
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        Task<Basket?> GetByUserIdAsync(string userId);
    }
}
