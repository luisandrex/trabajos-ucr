using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.MedicalRecords.Constants
{
    public partial class RecordSummary
    {
        public List<string> Values { get; set; }
        public List<string> Content { get; set; }
        public void LoadValues(RecordViewModel record)
        {
            Values = new List<string> { record.Nombre + " " + record.PrimerApellido + " " + record.SegundoApellido, record.Cedula };
            Content = new List<string> { "Nombre completo: ", "Cédula: " };
        }

        public void LoadPatientValues(Paciente patient) 
        {
            Values = new List<string> { patient.Nombre + " " + patient.PrimerApellido + " " + patient.SegundoApellido, patient.Cédula };
            Content = new List<string> { "Nombre completo: ", "Cédula: " };
        }
    }
}
