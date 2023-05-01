namespace Watch.DataAccess.UI.Interfaces
{
    public interface IMovementTypeRepository : IGenericRepository<MovementType>
    {
        Task<List<Sale>> GetSalesAsync();
    }
}
