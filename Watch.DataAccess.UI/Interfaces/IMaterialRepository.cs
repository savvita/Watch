namespace Watch.DataAccess.UI.Interfaces
{
    public interface IMaterialRepository : IGenericRepository<Material>
    {
        Task<List<Sale>> GetSalesAsync(string type);
    }
}
