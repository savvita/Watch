using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public GenderRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Gender?> CreateAsync(Gender entity)
        {
            var model = await _db.Genders.CreateAsync((GenderModel)entity);
            return model != null ? new Gender(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.GenderId == id)).ToList().ForEach(w => w.GenderId = null);
            return await _db.Genders.DeleteAsync(id);
        }

        public async Task<IEnumerable<Gender>> GetAsync()
        {
            return (await _db.Genders.GetAsync()).Select(model => new Gender(model));
        }

        public async Task<Gender?> GetAsync(int id)
        {
            var model = await _db.Genders.GetAsync(id);

            return model != null ? new Gender(model) : null;
        }

        public async Task<Gender> UpdateAsync(Gender entity)
        {
            var model = await _db.Genders.UpdateAsync((GenderModel)entity);

            return new Gender(model);
        }
    }
}
