using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.MedicalRecords
{
    public class RecordViewModel
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public DateTime FechaCreacion { get; set; }
        public string Clinica { get; set; }

        public string NombreMedico { get; set; }
        public string PrimerApellidoMedico { get; set; }
        public string SegundoApellidoMedico { get; set; }

        public int IdExpediente { get; set; }

    }
}
