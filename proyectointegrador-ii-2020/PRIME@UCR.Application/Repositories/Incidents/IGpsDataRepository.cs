#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IGpsDataRepository
    {
        Task<IEnumerable<IncidentGpsData>> GetGpsDataAsync(IEnumerable<OngoingIncident> incidents);
        Task<IEnumerable<Modalidad>> GetUnitTypeFiltersAsync();
        Task<IEnumerable<Estado>> GetStateFiltersAsync();
    }
}
