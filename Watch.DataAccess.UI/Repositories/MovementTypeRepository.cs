﻿using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class MovementTypeRepository : IMovementTypeRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public MovementTypeRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<MovementType?> CreateAsync(MovementType entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.MovementTypes.CreateAsync((MovementTypeModel)entity);
            return model != null ? new MovementType(model) : null;

        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var models = await _db.MovementTypes.GetSalesAsync();
            var entities = new List<Sale>();

            foreach (var model in models)
            {
                if (model != null)
                {
                    entities.Add(new Sale(model));
                }
            }

            return entities;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            (await _db.Watches.Where(w => w.MovementTypeId == id)).ToList().ForEach(w => w.MovementTypeId = null);
            return await _db.MovementTypes.DeleteAsync(id);
        }

        public async Task<IEnumerable<MovementType>> GetAsync()
        {
            return (await _db.MovementTypes.GetAsync()).Select(model => new MovementType(model));
        }

        public async Task<MovementType?> GetAsync(int id)
        {
            var model = await _db.MovementTypes.GetAsync(id);

            return model != null ? new MovementType(model) : null;
        }

        public async Task<MovementType> UpdateAsync(MovementType entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.MovementTypes.UpdateAsync((MovementTypeModel)entity);

            return new MovementType(model);
        }
    }
}
