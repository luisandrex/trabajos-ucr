using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class ReferenciaCita
    {
        public int IdCita1 { get; set; }

        public int IdCita2 { get; set; }

        public string Especialidad { get; set; }

        public Cita Cita1 { get; set; }

        public Cita Cita2 { get; set; }

        public EspecialidadMedica EspecialidadMedica { get; set; }

    }
}
