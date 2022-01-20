using System;
using System.ComponentModel;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
{
    public enum DetailsTab
    {
        [Description("Resumen")]
        Info,
        [Description("Origen")]
        Origin,
        [Description("Destino")]
        Destination,
        [Description("Paciente")]
        Patient,
        [Description("Asignación")]
        Assignment,
        [Description("Multimedia")]
        Multimedia,
        [Description("Listas de chequeo")]
        Checklist,
    }
}