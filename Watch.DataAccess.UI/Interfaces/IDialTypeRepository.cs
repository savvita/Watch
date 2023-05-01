namespace Watch.DataAccess.UI.Interfaces
{
    public interface IDialTypeRepository : IGenericRepository<DialType>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
