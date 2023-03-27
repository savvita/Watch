namespace Watch.DataAccess.UI.Interfaces
{
    public interface ICityRepository
    {
        Task<List<City>> GetAsync();
        Task<bool> UpdateAsync(string apiKey, string url);
        Task<DateTime?> GetLastUpdateAsync();
    }
}
