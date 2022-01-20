using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class DistrictRepository : RepoDbRepository<Distrito, int>, IDistrictRepository
    {
        public DistrictRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public async Task<IEnumerable<Distrito>> GetDistrictsByCantonIdAsync(int cantonId)
        {
            return await _db.Districts
                .AsNoTracking()
                .Where(d => d.IdCanton == cantonId)
                .ToListAsync();
        }

        public async Task<Distrito> GetDistrictWithFullLocationById(int distrcitId)
        {
            return await _db.Districts
                .Include(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .ThenInclude(p => p.Pais)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == distrcitId);
        }
    }
}