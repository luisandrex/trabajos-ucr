using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class Expediente
    {
        public int Id { get; set; }
        public string CedulaPaciente { get; set; } //fk-paciente
        public string CedulaMedicoDuenno { get; set; } //fk-medico
        public DateTime FechaCreacion { get; set; }
        public string Clinica { get; set; }
        public Médico Medico { get; set; }
        public Paciente Paciente { get; set; }
        public List<Alergias> Alergias { get; set; }
        public List<Antecedentes> Antecedentes { get; set; }
        public List<PadecimientosCronicos> PadecimientosCronicos { get; set; }
        public List<Cita> Citas { get; set; }
    }
}
