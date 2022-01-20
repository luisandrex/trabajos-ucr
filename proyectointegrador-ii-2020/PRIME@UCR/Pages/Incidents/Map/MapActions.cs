#nullable enable
using System.Collections.Generic;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Pages.Incidents.Map
{
    public abstract record MapAction
    {
    }

    public record LoadingGpsDataAction : MapAction
    {
    }

    public record LoadGpsDataAction : MapAction
    {
    }

    public record LoadGpsDataWithUnitFilterAction : MapAction
    {
        public Modalidad? UnitTypeFilter { get; init; }
    }

    public record LoadGpsDataWithStateFilterAction : MapAction
    {
        public Estado? StateFilter { get; init; }
    }

    public record LoadGpsDataSuccessfulAction : MapAction
    {
        public LoadGpsDataSuccessfulAction(
            IEnumerable<IncidentGpsData> gpsData,
            IEnumerable<Modalidad> unitTypeFilters,
            Modalidad? selectedUnitFilter,
            IEnumerable<Estado> stateFilters,
            Estado? selectedStateFilter)
        => (GpsData, UnitTypeFilters, SelectedUnitFilter, StateFilters, SelectedStateFilter)
         = (gpsData, unitTypeFilters, selectedUnitFilter, stateFilters, selectedStateFilter);
        
            
        
        public IEnumerable<IncidentGpsData> GpsData { get; init; }
        public IEnumerable<Modalidad> UnitTypeFilters { get; init; }
        public Modalidad? SelectedUnitFilter { get; init; }
        public IEnumerable<Estado> StateFilters { get; init; }
        public Estado? SelectedStateFilter { get; init; }
    }
}
