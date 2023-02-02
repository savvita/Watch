using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class ProducerRepository : GenericRepository<ProducerModel>, IProducerRepository
    {
        public ProducerRepository(WatchDbContext context) : base(context)
        {
        }
    }
}
