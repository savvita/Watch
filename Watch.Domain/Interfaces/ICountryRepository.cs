using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface ICountryRepository : IGenericRepository<CountryModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
