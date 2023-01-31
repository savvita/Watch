using Watch.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Watch.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly WatchDbContext _db;

        public GenericRepository(WatchDbContext db)
        {
            _db = db;
        }

        public async Task<T?> CreateAsync(T entity)
        {
            entity = (await _db.Set<T>().AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();

            return result.Entity;
        }
    }
}
