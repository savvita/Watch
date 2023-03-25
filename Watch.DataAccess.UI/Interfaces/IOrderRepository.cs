namespace Watch.DataAccess.UI.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAsync(OrderFilter filters);
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetByManagerIdAsync(string managerId);
        Task<Order?> CreateAsync(Basket basket, OrderAdditionalInfo info);
        Task<bool> SetOrderStatusAsync(int id, int statusId);
        Task<bool> CloseOrderAsync(int id);
        Task<bool> CancelOrderAsync(int id);
        Task<bool> AcceptOrderAsync(int orderId, string managerId);
    }
}
