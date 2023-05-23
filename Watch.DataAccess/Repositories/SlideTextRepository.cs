using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class SlideTextRepository : GenericRepository<TextModel>, ISlideTextRepository
    {
        public SlideTextRepository(WatchDbContext db) : base(db)
        {
        }

        private bool CheckRowVersion(byte[] db, byte[] updated)
        {
            if (db.Length != updated.Length)
            {
                return false;
            }
            int length = db.Length;
            for (int i = 0; i < length; i++)
            {
                if (db[i] != updated[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(TextModel entity)
        {
            try
            {
                var model = (await _db.SlideTexts.FirstAsync(x => x.Id == entity.Id));
                model.Text = entity.Text.Trim();
                model.Left = entity.Left;
                model.Top = entity.Top;
                model.FontSize = entity.FontSize;
                model.FontColor = entity.FontColor;
                model.SlideId = entity.SlideId;

                if (!CheckRowVersion(model.RowVersion, entity.RowVersion))
                {
                    throw new DbUpdateConcurrencyException();
                }

                _db.SlideTexts.Update(model);
                await _db.SaveChangesAsync();

                return new ConcurrencyUpdateResultModel()
                {
                    Code = 200,
                    Message = "Ok"
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                var model = await _db.SlideTexts.FindAsync(entity.Id);

                if (model == null)
                {
                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 404,
                        Message = "The entry was deleted"
                    };
                }

                return new ConcurrencyUpdateResultModel()
                {
                    Code = 409,
                    Message = "The entry was modified by another user"
                };
            }
            catch
            {
                return new ConcurrencyUpdateResultModel()
                {
                    Code = 500,
                    Message = "Internal Server Error"
                };
            }
        }
    }
}
