using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IDistrictRepository : IGenericRepository<Distrito, int>
    {
        Task<IEnumerable<Distrito>> GetDistrictsByCantonIdAsync(int cantonId);
        Task<Distrito> GetDistrictWithFullLocationById(int distrcitId);
    }
}