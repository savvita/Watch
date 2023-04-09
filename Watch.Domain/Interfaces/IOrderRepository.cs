using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderModel>
    {
        Task<List<OrderModel?>> GetAsync(OrderFilterModel filters);
        Task<IEnumerable<OrderModel>> GetByUserIdAsync(string userId);
        Task<IEnumerable<OrderModel>> GetByManagerIdAsync(string managerId);
        Task<bool> SetOrderStatusAsync(int id, int statusId);
        Task<bool> SetENAsync(int id, string en);
        Task<bool> CancelOrderAsync(int id);
        Task<bool> CloseOrderAsync(int id);
        Task<bool> AcceptOrderAsync(int orderId, string managerId);
    }
}
