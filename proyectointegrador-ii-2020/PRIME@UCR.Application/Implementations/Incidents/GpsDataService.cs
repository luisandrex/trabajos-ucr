#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.Implementations.Incidents
{
    public class GpsDataService : IGpsDataService
    {
        private readonly IGpsDataRepository _gpsRepository;
        private readonly IIncidentRepository _incidentRepository;

        public GpsDataService(IGpsDataRepository gpsRepository, IIncidentRepository incidentRepository)
        {
            _gpsRepository = gpsRepository;
            _incidentRepository = incidentRepository;
        }

        public async Task<IEnumerable<IncidentGpsData>> GetGpsDataAsync(Modalidad? unitType, Estado? state)
        {
            var ongoingIncidents
                = await _incidentRepository.GetOngoingIncidentsAsync(unitType, state);

            return await _gpsRepository.GetGpsDataAsync(ongoingIncidents);
        }

        public async Task<IEnumerable<Modalidad>> GetGpsDataUnitFiltersAsync()
        {
            return await _gpsRepository.GetUnitTypeFiltersAsync();
        }

        public async Task<IEnumerable<Estado>> GetGpsDataStateFiltersAsync()
        {
            return await _gpsRepository.GetStateFiltersAsync();
        }
    }
}
