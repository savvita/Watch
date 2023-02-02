using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IBasketDetailRepository : IGenericRepository<BasketDetailModel>
    {
        Task<IEnumerable<BasketDetailModel>> GetByBasketIdAsync(int basketId);
    }
}
