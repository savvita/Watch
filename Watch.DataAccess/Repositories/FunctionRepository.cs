using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class FunctionRepository : GenericRepository<FunctionModel>, IFunctionRepository
    {
        public FunctionRepository(WatchDbContext db) : base(db)
        {
        }
    }
}
