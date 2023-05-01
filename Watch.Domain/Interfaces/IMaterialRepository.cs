using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IMaterialRepository : IGenericRepository<MaterialModel>
    {
        Task<List<SaleModel>> GetSalesAsync(string type);
    }
}
