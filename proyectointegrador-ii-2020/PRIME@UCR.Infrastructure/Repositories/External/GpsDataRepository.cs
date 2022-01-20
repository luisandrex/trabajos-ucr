#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Infrastructure.Repositories.External
{
    public class GpsDataRepository : IGpsDataRepository
    {
        private record GpsDirection(double Long, double Lat);
        
        private readonly IDictionary<string, Tuple<IncidentGpsData, GpsDirection>> _lastKnownCoordinates
            = new Dictionary<string, Tuple<IncidentGpsData, GpsDirection>>();

        private readonly Random _rand = new Random();

        private const double kMinLatitude = 9.176097;
        private const double kMaxLatitude = 9.927069;
        private const double kMinLongitude = -84.052347;
        private const double kMaxLongitude = -82.974313;
        private const double kMinLatitudeRange = -0.005;
        private const double kMaxLatitudeRange = 0.005;
        private const double kMinLongitudeRange = -0.05;
        private const double kMaxLongitudeRange = 0.05;

        /// <summary>
        /// Gets the next gps position for an ongoing incident and stores it locally.
        /// </summary>
        private IncidentGpsData GetNextGpsPosition(OngoingIncident incident)
        {
            if (_lastKnownCoordinates.TryGetValue(incident.Code, out var gpsData))
            {
                var nextGpsData = gpsData.Item1 with
                    {
                        CurrentLongitude = gpsData.Item1.CurrentLongitude + gpsData.Item2.Long,
                        CurrentLatitude = gpsData.Item1.CurrentLatitude + gpsData.Item2.Lat
                    };

                _lastKnownCoordinates[incident.Code] = new Tuple<IncidentGpsData, GpsDirection>(nextGpsData, gpsData.Item2); // persist data

                return nextGpsData;
            }

            var newGpsData = new IncidentGpsData
            {
                IncidentCode = incident.Code,
                OngoingIncident = incident,
                CurrentLongitude = RandomLongitude(newPos: true),
                CurrentLatitude = RandomLatitude(newPos: true),
            };

            // persist data
            _lastKnownCoordinates.Add(new (incident.Code,
                new(newGpsData, new GpsDirection(RandomLongitude(false), RandomLatitude(false)))));

            return newGpsData;
        }
        
        /// <summary>
        /// Gets the next current GPS data for each ongoing incident in the <paramref name="incidents"/> list.
        /// </summary>
        /// <param name="incidents">List of ongoing incidents.</param>
        /// <returns>New list corresponding to the current GPS data for each ongoing incident.</returns>
        public Task<IEnumerable<IncidentGpsData>> GetGpsDataAsync(IEnumerable<OngoingIncident> incidents)
        {
            // O(nlog(n)) time
            return Task.FromResult(
                incidents.Select(GetNextGpsPosition));
        }

        /// <summary>
        /// Gets available list of unit type filters.
        /// </summary>
        public Task<IEnumerable<Modalidad>> GetUnitTypeFiltersAsync()
        {
            return Task.FromResult(new List<Modalidad>
            {
                new Modalidad { Tipo = "Aéreo" },
                new Modalidad { Tipo = "Marítimo" },
                new Modalidad { Tipo = "Terrestre" },
            }.AsEnumerable());
        }

        /// <summary>
        /// Gets available list of state filters.
        /// </summary>
        public Task<IEnumerable<Estado>> GetStateFiltersAsync()
        {
            return Task.FromResult(IncidentStates.OngoingStates.AsEnumerable());
        }

        private double RandomLongitude(bool newPos)
        {
            return newPos
                ? _rand.NextDouble() * (kMaxLongitude - kMinLongitude) + kMinLongitude
                : _rand.NextDouble() * (kMaxLongitudeRange - kMinLongitudeRange) + kMinLongitudeRange;
        }


        private double RandomLatitude(bool newPos)
        {
            return newPos
                ? _rand.NextDouble() * (kMaxLatitude - kMinLatitude) + kMinLatitude
                : _rand.NextDouble() * (kMaxLatitudeRange - kMinLatitudeRange) + kMinLatitudeRange;
        }
    }
}
