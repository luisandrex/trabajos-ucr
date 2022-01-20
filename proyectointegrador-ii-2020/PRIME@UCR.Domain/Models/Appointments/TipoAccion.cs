using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class TipoAccion
    {
        public string Nombre { get; set; }
        public bool EsDeIncidente { get; set; }
        public bool EsDeCitaMedica { get; set; }

    }
}
