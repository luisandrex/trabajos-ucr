#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.Services.Incidents
{
    /// <summary>
    /// Manages realtime GPS data for ongoing incidents.
    /// </summary>
    public interface IGpsDataService
    {
        /// <summary>
        /// Gets all ongoing incidents with gps data based on filters.
        /// </summary>
        Task<IEnumerable<IncidentGpsData>> GetGpsDataAsync(Modalidad? unitType, Estado? state);
        Task<IEnumerable<Modalidad>> GetGpsDataUnitFiltersAsync();
        Task<IEnumerable<Estado>> GetGpsDataStateFiltersAsync();
    }
}
