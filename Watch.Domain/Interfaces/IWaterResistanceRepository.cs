using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IWaterResistanceRepository : IGenericRepository<WaterResistanceModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
