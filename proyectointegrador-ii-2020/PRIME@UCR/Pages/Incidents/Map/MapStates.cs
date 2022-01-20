#nullable enable
using System.Collections.Generic;
using Fluxor;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Pages.Incidents.Map
{
    public abstract record MapState
    {
    }

    public record LoadingMapState : MapState
    {
    }

    public record LoadedMapState : MapState
    {
        public IEnumerable<IncidentGpsData> GpsData { get; init; }
        public IEnumerable<Modalidad> AvailableUnitFilters { get; init; }
        public IEnumerable<Estado> AvailableStateFilters { get; init; }
        public Modalidad? UnitFilter { get; init; }
        public Estado? StateFilter { get; init; }
        public GpsPoint Center { get; init; }
    }

    public record GpsPoint
    {
        public double Longitude { get; init; }
        public double Latitude { get; init; }
    }

    public class MapFeature : Feature<MapState>
    {
        public override string GetName() => nameof(MapState);
        protected override MapState GetInitialState() => new LoadingMapState();
    }
}
