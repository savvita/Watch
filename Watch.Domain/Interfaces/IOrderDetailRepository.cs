using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetailModel>
    {
        Task<IEnumerable<OrderDetailModel>> GetByOrderIdAsync(int orderId);
    }
}
