using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class EspecialidadMedica
    {
        public string Nombre { get; set; }

        public List<SeEspecializa> Especialistas { get; set; }
    }
}
