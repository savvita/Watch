using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IStyleRepository : IGenericRepository<StyleModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
