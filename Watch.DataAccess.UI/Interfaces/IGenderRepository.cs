namespace Watch.DataAccess.UI.Interfaces
{
    public interface IGenderRepository : IGenericRepository<Gender>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
