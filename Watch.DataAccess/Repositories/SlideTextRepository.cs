using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class SlideTextRepository : GenericRepository<TextModel>, ISlideTextRepository
    {
        public SlideTextRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
