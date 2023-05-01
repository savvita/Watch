using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IGenderRepository : IGenericRepository<GenderModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
