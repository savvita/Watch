using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IGlassTypeRepository : IGenericRepository<GlassTypeModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
