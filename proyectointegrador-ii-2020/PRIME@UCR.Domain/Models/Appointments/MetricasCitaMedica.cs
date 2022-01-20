using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class MetricasCitaMedica
    {
        public int Id { get; set; }

        public int CitaId { get; set; }         //fk-Cita

        public string Presion { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }


        public Cita Cita { get; set; }
    }
}
