using Microsoft.AspNetCore.Identity;
using Watch.DataAccess.UI.Interfaces;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public ProducerRepository(WatchDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = new UnitOfWorks.UnitOfWorks(context, userManager, roleManager);
        }
        public async Task<Producer?> CreateAsync(Producer entity)
        {
            var model = await _db.Producers.CreateAsync((ProducerModel)entity);

            return model != null ? new Producer(model) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Producers.DeleteAsync(id);
        }

        public async Task<IEnumerable<Producer>> GetAsync()
        {
            return (await _db.Producers.GetAsync()).Select(model => new Producer(model));
        }

        public async Task<Producer?> GetAsync(int id)
        {
            var model = await _db.Producers.GetAsync(id);

            return model != null ? new Producer(model) : null;
        }

        public async Task<Producer> UpdateAsync(Producer entity)
        {
            var model = await _db.Producers.UpdateAsync((ProducerModel)entity);

            return new Producer(model);
        }
    }
}
