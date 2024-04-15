using DataAccess.Repositories.Implementation;
using Repository.Abstraction;
using Repository.Entity;

namespace Repository.Implementation;

public class LocationRepository : GenericRepository<BankLocation>, ILocationRepository
{
    public LocationRepository(ExchangeBotDbContext context) : base(context)
    {
    }
}
