namespace Watch.DataAccess.UI.Interfaces
{
    public interface IWaterResistanceRepository : IGenericRepository<WaterResistance>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
