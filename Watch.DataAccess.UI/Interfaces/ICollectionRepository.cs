namespace Watch.DataAccess.UI.Interfaces
{
    public interface ICollectionRepository : IGenericRepository<Collection>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
