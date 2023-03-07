using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class ImageRepository : GenericRepository<ImageModel>, IImageRepository
    {
        public ImageRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
