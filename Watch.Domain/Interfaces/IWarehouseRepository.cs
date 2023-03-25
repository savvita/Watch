
using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<List<WarehouseModel>> GetBySettlementRefAsync(string settlementRefRef);
        Task<bool> UpdateAsync(string apiKey, string url);
        Task<DateTime?> GetLastUpdateAsync();
    }
}
