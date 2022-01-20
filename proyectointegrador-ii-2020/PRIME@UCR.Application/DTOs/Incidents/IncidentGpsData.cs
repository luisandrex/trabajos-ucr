using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.DTOs.Incidents
{
    // Represents current GPS data for the unit of an ongoing incident in the field
    public record IncidentGpsData
    {
        public double CurrentLongitude { get; init; }
        public double CurrentLatitude { get; init; }
        public string IncidentCode { get; init; }
        public OngoingIncident OngoingIncident { get; init; }
    }
}
