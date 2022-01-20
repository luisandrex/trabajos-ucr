using PRIME_UCR.Domain.Models.Incidents;
using System.Collections.Generic;

namespace PRIME_UCR.Domain.Constants
{
    public static class IncidentStates
    {
        // TODO: fill with known states
        public static readonly Estado InCreationProcess = new Estado { Nombre = "En proceso de creación" };
        public static readonly Estado Created = new Estado { Nombre = "Creado" };
        public static readonly Estado Rejected = new Estado { Nombre = "Rechazado" };
        public static readonly Estado Approved = new Estado { Nombre = "Aprobado" };
        public static readonly Estado Assigned = new Estado { Nombre = "Asignado" };
        public static readonly Estado Preparing = new Estado { Nombre = "En preparación" };
        public static readonly Estado InOriginRoute = new Estado { Nombre = "En ruta a origen" };
        public static readonly Estado PatientInOrigin = new Estado { Nombre = "Paciente recolectado en origen" };
        public static readonly Estado InRoute = new Estado { Nombre = "En traslado" };
        public static readonly Estado Delivered = new Estado { Nombre = "Entregado" };
        public static readonly Estado Reactivated = new Estado { Nombre = "Reactivación" };
        public static readonly Estado Done = new Estado { Nombre = "Finalizado" };
        public static readonly List<Estado> IncidentStatesList = new List<Estado>
        {
            InCreationProcess,
            Created,
            Rejected,
            Approved,
            Assigned,
            Preparing,
            InOriginRoute,
            PatientInOrigin,
            InRoute,
            Delivered,
            Reactivated,
            Done
        };

        public static readonly List<Estado> OngoingStates = new List<Estado>
        {
            Preparing,
            InOriginRoute,
            PatientInOrigin,
            InRoute,
            Delivered,
            Reactivated,
        };
    }
}
