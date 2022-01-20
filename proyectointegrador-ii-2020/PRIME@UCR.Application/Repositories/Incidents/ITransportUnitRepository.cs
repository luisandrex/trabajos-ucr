using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface ITransportUnitRepository : IGenericRepository<UnidadDeTransporte, string>
    {
        Task<IEnumerable<UnidadDeTransporte>> GetAllTransporUnitsByMode(string mode);
        Task<UnidadDeTransporte> GetTransporUnitByIncidentIdAsync(string incidentId);

    }
}
