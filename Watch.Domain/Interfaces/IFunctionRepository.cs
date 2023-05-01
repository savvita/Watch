using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IFunctionRepository : IGenericRepository<FunctionModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
