using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IColorRepository : IGenericRepository<ColorModel>
    {
        Task<List<SaleModel>> GetSalesAsync(string type);
    }
}
