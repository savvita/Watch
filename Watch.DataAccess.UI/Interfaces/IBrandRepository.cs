namespace Watch.DataAccess.UI.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
