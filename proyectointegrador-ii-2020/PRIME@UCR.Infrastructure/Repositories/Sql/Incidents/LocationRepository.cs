using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class LocationRepository : GenericRepository<Ubicacion, int>, ILocationRepository
    {
        public LocationRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }
    }
}