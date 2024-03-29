﻿using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public CountryRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Country?> CreateAsync(Country entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Countries.CreateAsync((CountryModel)entity);
            return model != null ? new Country(model) : null;

        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var models = await _db.Countries.GetSalesAsync();
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
            return await _db.Countries.DeleteAsync(id);
        }

        public async Task<IEnumerable<Country>> GetAsync()
        {
            return (await _db.Countries.GetAsync()).Select(model => new Country(model));
        }

        public async Task<Country?> GetAsync(int id)
        {
            var model = await _db.Countries.GetAsync(id);

            return model != null ? new Country(model) : null;
        }

        public async Task<Country> UpdateAsync(Country entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Countries.UpdateAsync((CountryModel)entity);

            return new Country(model);
        }
    }
}
