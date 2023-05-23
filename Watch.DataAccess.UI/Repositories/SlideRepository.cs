using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class SlideRepository : ISlideRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public SlideRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Slide?> CreateAsync(Slide entity)
        {
            var model = await _db.Slides.CreateAsync((SlideModel)entity);
            return model != null ? new Slide(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _db.Slides.GetAsync(id);
            if(model == null)
            {
                return false;
            }

            model.IsActive = false;

            var res = await _db.Slides.UpdateConcurrencyAsync(model);

            return res.Code == 200;
        }

        public async Task<IEnumerable<Slide>> GetAsync()
        {
            return (await _db.Slides.GetAsync()).Select(model => new Slide(model));
        }

        public async Task<IEnumerable<Slide>> GetAsync(bool activeOnly)
        {
            return (await _db.Slides.GetAsync(activeOnly)).Select(model => new Slide(model));
        }

        public async Task<Slide?> GetAsync(int id)
        {
            var model = await _db.Slides.GetAsync(id);

            return model != null ? new Slide(model) : null;
        }

        public async Task<Slide> UpdateAsync(Slide entity)
        {
            var model = await _db.Slides.UpdateAsync((SlideModel)entity);

            return new Slide(model);
        }

        public async Task<ConcurrencyUpdateResult> UpdateConcurrencyAsync(Slide entity)
        {
            var model = await _db.Slides.UpdateConcurrencyAsync((SlideModel)entity);

            return new Models.ConcurrencyUpdateResult()
            {
                Code = model.Code,
                Message = model.Message
            };
        }
    }
}
