using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderModel>
    {
        Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId);
        Task<bool> CloseOrderAsync(int id);
    }
}
