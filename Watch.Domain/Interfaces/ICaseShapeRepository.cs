using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface ICaseShapeRepository : IGenericRepository<CaseShapeModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
