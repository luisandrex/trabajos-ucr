using System;
using System.ComponentModel;

namespace PRIME_UCR.Components.MedicalRecords
{
    public enum DetailsTab
    {
        [Description("Informaci�n general")]
        Info,
        [Description("Citas anteriores")]
        Appointments,
        [Description("Antecedentes M�dicos, Alergias y Padecimientos cr�nicos")]
        MedicalBackgroundTab
    }
}