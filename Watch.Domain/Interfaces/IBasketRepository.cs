using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IBasketRepository : IGenericRepository<BasketModel>
    {
        Task<BasketModel?> GetByUserIdAsync(string userId);
    }
}
