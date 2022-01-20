using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Domain.Models
{
    public class Cita
    {
        public int Id { get; set; }

        public DateTime FechaHoraCreacion { get; set; }

        public DateTime FechaHoraEstimada { get; set; }

        public List<Accion> Acciones { get; set; }

        public int? IdExpediente { get; set; }
        public Expediente Expediente { get; set; }

        public List<MetricasCitaMedica> Metricas { get; set; }

        public List<Incidente> Incidentes { get; set; }

        public List<CitaMedica> CitasMedicas { get; set; }

        public List<ReferenciaCita> Referencias { get; set; }

    }
}
