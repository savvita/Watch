using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderModel>
    {
        Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId);
        Task<IEnumerable<OrderModel>> GetByManagerIdAsync(string managerId);
        Task<bool> SetOrderStatusAsync(int id, int statusId);
        
        //TODO delete this method
        Task<bool> CloseOrderAsync(int id);
    }
}
