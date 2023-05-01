using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface ICollectionRepository : IGenericRepository<CollectionModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
