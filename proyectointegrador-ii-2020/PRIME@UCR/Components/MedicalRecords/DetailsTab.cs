using System;
using System.ComponentModel;

namespace PRIME_UCR.Components.MedicalRecords
{
    public enum DetailsTab
    {
        [Description("Información general")]
        Info,
        [Description("Citas anteriores")]
        Appointments,
        [Description("Antecedentes Médicos, Alergias y Padecimientos crónicos")]
        MedicalBackgroundTab
    }
}