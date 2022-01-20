using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.MedicalAppointments
{
    public enum MADetailsTab
    {

        [Description("Información General")]
        General,
        [Description("Recetas Médicas")]
        Recetas,
        [Description("Contenido Multimedia")]
        Multimedia,
        [Description("Mediciones")]
        Metricas,
        [Description("Referencias")]
        Referencias
    }
}
