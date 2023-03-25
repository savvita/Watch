
using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface ICityRepository
    {
        Task<List<CityModel>> GetAsync();
        Task<bool> UpdateAsync(string apiKey, string url);
        Task<DateTime?> GetLastUpdateAsync();
    }
}
