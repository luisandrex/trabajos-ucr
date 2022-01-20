using System.Collections.Generic;
using System.Linq;
using Fluxor;
using PRIME_UCR.Application.DTOs.Incidents;
using Radzen;

namespace PRIME_UCR.Pages.Incidents.Map
{
    public static class MapReducers
    {
        private static readonly GpsPoint kDefaultCenter = new GpsPoint
        {
            Latitude = 9.927069,
            Longitude = -83.188547
        };

        [ReducerMethod]
        public static MapState ReduceLoadedGpsDataSuccessfulAction(MapState oldState,
                                                                   LoadGpsDataSuccessfulAction action)
            => new LoadedMapState
            {
                GpsData = action.GpsData,
                Center = CalculateMapCenter(action.GpsData),
                UnitFilter = action.SelectedUnitFilter,
                AvailableUnitFilters = action.UnitTypeFilters,
                AvailableStateFilters = action.StateFilters,
                StateFilter = action.SelectedStateFilter
            };

        [ReducerMethod]
        public static MapState ReduceLoadingGpsDataAction(MapState oldState, LoadingGpsDataAction action)
            => new LoadingMapState();

        private static GpsPoint CalculateMapCenter(IEnumerable<IncidentGpsData> gpsData)
        {
            var incidentGpsData = gpsData.ToList();

            if (!incidentGpsData.Any()) return kDefaultCenter;

            var centerLat = incidentGpsData.Average(i => i.CurrentLatitude);
            var centerLong = incidentGpsData.Average(i => i.CurrentLongitude);

            return new GpsPoint
            {
                Latitude = centerLat,
                Longitude = centerLong
            };
        }
    }
}
