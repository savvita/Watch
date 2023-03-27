namespace Watch.DataAccess.UI.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetBySettlementRefAsync(string settlementRefRef);
        Task<bool> UpdateAsync(string apiKey, string url);
        Task<DateTime?> GetLastUpdateAsync();
    }
}
