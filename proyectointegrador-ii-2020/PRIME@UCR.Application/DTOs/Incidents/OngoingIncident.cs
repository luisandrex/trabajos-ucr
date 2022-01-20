using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.DTOs.Incidents
{

    /// <summary>
    /// Represents basic data relevant to an incident while it is ongoing.
    /// </summary>
    public class OngoingIncident
    {
        public string Code { get; init; }
        public Incidente Incident { get; init; }
        public UnidadDeTransporte TransportUnit { get; init; }
        public Ubicacion Origin { get; init; }
        public Ubicacion Destination { get; init; }
        public Modalidad UnitType { get; init; }
    }
}
