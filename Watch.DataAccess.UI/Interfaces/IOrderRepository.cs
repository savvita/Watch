namespace Watch.DataAccess.UI.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetByManagerIdAsync(string managerId);
        Task<Order?> CreateAsync(Basket basket);
        Task<bool> SetOrderStatusAsync(int id, int statusId);
        Task<bool> CloseOrderAsync(int id);
        Task<bool> CancelOrderAsync(int id);
    }
}
