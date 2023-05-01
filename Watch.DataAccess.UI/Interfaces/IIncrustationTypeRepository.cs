namespace Watch.DataAccess.UI.Interfaces
{
    public interface IIncrustationTypeRepository : IGenericRepository<IncrustationType>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
