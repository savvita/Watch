using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class SlideRepository : GenericRepository<SlideModel>, ISlideRepository
    {
        public SlideRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<SlideModel?> CreateAsync(SlideModel entity)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (entity.PromotionId != null)
                    {
                        var promotion = await _db.Promotions.FirstAsync(x => x.Id == entity.PromotionId);
                        if(!promotion.IsActive)
                        {
                            entity.IsActive = false;
                        }
                    }

                    await CreateSlideTexts(entity);

                    entity = (await _db.Slides.AddAsync(entity)).Entity;

                    if(entity.IsActive)
                    {
                        await HandleIndexes(entity);
                    }

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return entity;
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }

        }

        private async Task HandleIndexes(SlideModel entity)
        {
            if(!entity.IsActive)
            {
                return;
            }

            await _db.Slides.Where(x => x.IsActive && x.Index >= entity.Index).ForEachAsync(x =>
            {
                int idx = x.Index + 1;
                if(idx <= 5)
                {
                    x.Index = idx;
                }
                else
                {
                    x.IsActive = false;
                }
            });
        }

        private async Task CreateSlideTexts(SlideModel entity)
        {
            var texts = new List<TextModel>(entity.Texts);
            entity.Texts.Clear();

            foreach (var text in texts)
            {
                if (text.FontSize < 0)
                {
                    continue;
                }
                text.Text = text.Text.Trim();
                if (text.Text.Length == 0)
                {
                    continue;
                }

                var item = (await _db.SlideTexts.AddAsync(text)).Entity;
                if (item != null)
                {
                    entity.Texts.Add(item);
                }
            }
        }

        public async Task<IEnumerable<SlideModel>> GetAsync(bool activeOnly)
        {
            return await Task.FromResult(_db.Slides.Where(x => x.IsActive == activeOnly).Include(x => x.Promotion).Include(x => x.Texts));
        }

        public new async Task<IEnumerable<SlideModel>> GetAsync()
        {
            return await Task.FromResult(_db.Slides.Include(x => x.Promotion).Include(x => x.Texts));
        }

        public new async Task<SlideModel?> GetAsync(int id)
        {
            return await _db.Slides.Include(x => x.Texts).Include(x => x.Promotion).FirstOrDefaultAsync();
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

        public async Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(SlideModel entity)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var model = (await _db.Slides.Include(x => x.Texts).FirstAsync(x => x.Id == entity.Id));
                    model.ImageUrl = entity.ImageUrl?.Trim();
                    model.PromotionId = entity.PromotionId;
                    model.Index = entity.Index;
                    model.IsActive = entity.IsActive;

                    if (entity.PromotionId != null)
                    {
                        var promotion = await _db.Promotions.FirstAsync(x => x.Id == entity.PromotionId);
                        if (!promotion.IsActive)
                        {
                            entity.IsActive = false;
                        }
                    }

                    await UpdateSlideTextsAsync(entity, model);

                    if (model.IsActive)
                    {
                        await HandleIndexes(model);
                    }

                    if(!CheckRowVersion(model.RowVersion, entity.RowVersion))
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    _db.Slides.Update(model);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 200,
                        Message = "Ok"
                    };
                }
                catch(DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
                    var model = await _db.Slides.FindAsync(entity.Id);

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
                    transaction.Rollback();
                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 500,
                        Message = "Internal Server Error"
                    };
                }
            }
        }
        private async Task<SlideModel> UpdateSlideTextsAsync(SlideModel newSlide, SlideModel oldSlide)
        {
            var texts = newSlide.Texts;
            newSlide.Texts.Clear();

            foreach(var text in texts)
            {
                if(text.Id == 0)
                {
                    if (text.FontSize < 0)
                    {
                        continue;
                    }
                    text.Text = text.Text.Trim();
                    if (text.Text.Length == 0)
                    {
                        continue;
                    }
                    var item = await _db.SlideTexts.AddAsync(text);
                    newSlide.Texts.Add(item.Entity);
                }
                else
                {
                    if (text.FontSize < 0)
                    {
                        continue;
                    }
                    text.Text = text.Text.Trim();
                    if (text.Text.Length == 0)
                    {
                        continue;
                    }

                    var item = await _db.SlideTexts.FirstOrDefaultAsync(x => x.Id == text.Id);

                    if(item == null)
                    {
                        continue;
                    }

                    item.Text = text.Text;
                    item.Left = text.Left;
                    item.Top = text.Top;
                    item.SlideId = text.SlideId;
                    item.FontColor = text.FontColor;
                    item.FontSize = text.FontSize;

                    var res = _db.SlideTexts.Update(item);
                    newSlide.Texts.Add(res.Entity);
                }
            }

            var selectedTexts = newSlide.Texts.Select(x => x.Id).ToList();
            var oldTexts = oldSlide.Texts.Select(x => x.Id).ToList();

            foreach (var text in _db.SlideTexts)
            {
                if (selectedTexts.Contains(text.Id))
                {
                    if (!oldTexts.Contains(text.Id))
                    {
                        oldSlide.Texts.Add(text);
                    }

                }
                else
                {
                    if (oldTexts.Contains(text.Id))
                    {
                        oldSlide.Texts.Remove(text);
                    }
                }
            }

            return oldSlide;
        }
    }
}
