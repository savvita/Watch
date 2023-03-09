using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class StyleRepository : GenericRepository<StyleModel>, IStyleRepository
    {
        public StyleRepository(WatchDbContext context) : base(context)
        {

        }
    }
}
