namespace Watch.DataAccess.UI.Interfaces
{
    public interface IFunctionRepository : IGenericRepository<Function>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
