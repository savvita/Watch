namespace Watch.DataAccess.UI.Interfaces
{
    public interface IGlassTypeRepository : IGenericRepository<GlassType>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
