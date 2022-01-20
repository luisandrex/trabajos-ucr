using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class RecordModel
    {
        public Expediente Expediente { get; set; }

        public Persona Persona { get; set; }

        public Paciente Paciente { get; set; }

        public string CedPaciente { get; set; }

        public string CedMedicoDuenno { get; set; }

        public string Clinica { get; set; }

        public Gender Sexo { get; set;  }

        public string Genero { get; set; }

        public CentroMedico CentroMedico { get; set; }


        public void set_to_null() {
            Expediente = null;
            Persona = null;
            Paciente = null;
            CedPaciente = null;
            CedMedicoDuenno = null;
            Clinica = null;
            Sexo = Gender.Unspecified;
            Genero = null;
            CentroMedico = null; 
        
        }
    }
}
