using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IIncrustationTypeRepository : IGenericRepository<IncrustationTypeModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
