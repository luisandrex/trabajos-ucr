using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class SeEspecializa
    {
        public string CedulaMedico { get; set; }

        public string NombreEspecialidad { get; set; }

        public Médico Medico { get; set; }

        public EspecialidadMedica Especialidad { get; set; }
    }
}
