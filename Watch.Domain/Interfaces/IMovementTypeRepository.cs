using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IMovementTypeRepository : IGenericRepository<MovementTypeModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
