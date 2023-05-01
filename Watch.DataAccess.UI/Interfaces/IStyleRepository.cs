namespace Watch.DataAccess.UI.Interfaces
{
    public interface IStyleRepository : IGenericRepository<Style>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
