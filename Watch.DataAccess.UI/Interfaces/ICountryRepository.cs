namespace Watch.DataAccess.UI.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
