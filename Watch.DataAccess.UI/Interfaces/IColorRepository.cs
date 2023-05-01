namespace Watch.DataAccess.UI.Interfaces
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        Task<List<Sale>> GetSalesAsync(string type);
    }
}
