namespace Watch.DataAccess.UI.Interfaces
{
    public interface ICaseShapeRepository : IGenericRepository<CaseShape>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
