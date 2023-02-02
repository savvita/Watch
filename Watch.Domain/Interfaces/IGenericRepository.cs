namespace Watch.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity);
        Task<IEnumerable<T>> GetAsync();
        Task<T?> GetAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
