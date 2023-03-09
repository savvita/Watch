using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public ImageRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Image?> CreateAsync(Image entity)
        {
            var model = await _db.Images.CreateAsync((ImageModel)entity);
            return model != null ? new Image(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Images.DeleteAsync(id);
        }

        public async Task<IEnumerable<Image>> GetAsync()
        {
            return (await _db.Images.GetAsync()).Select(model => new Image(model));
        }

        public async Task<Image?> GetAsync(int id)
        {
            var model = await _db.Images.GetAsync(id);

            return model != null ? new Image(model) : null;
        }

        public async Task<Image> UpdateAsync(Image entity)
        {
            var model = await _db.Images.UpdateAsync((ImageModel)entity);

            return new Image(model);
        }
    }
}
