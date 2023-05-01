namespace Watch.DataAccess.UI.Interfaces
{
    public interface IStrapTypeRepository : IGenericRepository<StrapType>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
