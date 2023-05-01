using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IStrapTypeRepository : IGenericRepository<StrapTypeModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
